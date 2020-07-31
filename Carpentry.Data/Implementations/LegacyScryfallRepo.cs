using Carpentry.Data.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Carpentry.Data.Models;
using Carpentry.Data.LegacyDataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScryfallSet = Carpentry.Data.LegacyDataContext.ScryfallSet;
using Carpentry.Data.LegacyModels;

namespace Carpentry.Data.Implementations
{
    public class LegacyScryfallRepo : ICardStringRepo
    {
        private readonly ILogger<LegacyScryfallRepo> _logger;
        private readonly HttpClient _client;
        private readonly ScryfallDataContext _scryContext;
        private readonly int _scryfallApiDelay;

        public LegacyScryfallRepo(
            ILogger<LegacyScryfallRepo> logger,
            HttpClient client,
            ScryfallDataContext scryContext
            )
        {
            _logger = logger;
            _client = client;
            _scryContext = scryContext;
            _scryfallApiDelay = 1000; //1 second
        }

        #region public members

        public async Task EnsureDatabaseCreated()
        {
            await _scryContext.Database.EnsureCreatedAsync();
        }

        public async Task<IQueryable<ScryfallMagicCard>> QueryCardsBySet(string setCode)
        {
            var allCardsQuery = await _scryContext.Cards.Where(x => x.Set.Code != null && x.Set.Code.ToLower() == setCode.ToLower()).ToListAsync();

            var query = allCardsQuery.Select(x => JObject.Parse(x.StringData).ToObject<ScryfallMagicCard>());

            return query.AsQueryable();
        }

        public async Task<IQueryable<ScryfallMagicCard>> QueryScryfallByName(string name, bool exclusive)
        {
            //What does SET SEARCH return?
            string endpoint;

            string nameParam = name.Replace(' ', '+');

            if (exclusive)
            {
                endpoint = $"https://api.scryfall.com/cards/search?q=!%22{nameParam}%22&unique=prints";
            }
            else
            {
                endpoint = $"https://api.scryfall.com/cards/search?q={nameParam}";
            }

            var responseString = await _client.GetStringAsync(endpoint);

            JObject responseObject = JObject.Parse(responseString);
            var cardResultData = responseObject.Value<JArray>("data").ToList();

            List<ScryfallMagicCard> mappedCards = MapScryfallDataToCards(cardResultData);

            return mappedCards.AsQueryable();
        }

        public async Task<ScryfallMagicCard> GetCardById(int multiverseId)
        {
            var cardInDb = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == multiverseId);

            if (cardInDb == null)
            {
                //Okay, if this card doesn't exist, I can't trust a query by MID (won't have all variants)
                //BUT, I can use the MID to find the set, THEN call EnsureSetExists, then return the card (wow)

                var rawScryfallCard = await QueryScryfallById(multiverseId);

                await EnsureSetExistsLocally(rawScryfallCard.Set);

                cardInDb = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == multiverseId);
            }

            var cardData = JObject.Parse(cardInDb.StringData).ToObject<ScryfallMagicCard>();

            return cardData;
        }

        public async Task<int> GetCardMultiverseId(string name, string setCode)
        {
            var card = await QueryScryfallBySetName(name, setCode);
            return card.MultiverseId;
        }




        public async Task AddScryCard(ScryfallCard card)
        {
            if (card.Id > 0)
            {
                throw new ArgumentException("Invalid card Id");
            }
            await _scryContext.Cards.AddAsync(card);
            await _scryContext.SaveChangesAsync();
        }

        public async Task AddScryCards(List<ScryfallCard> cards)
        {
            //if (card.Id > 0)
            //{
            //    throw new ArgumentException("Invalid card Id");
            //}
            await _scryContext.AddRangeAsync(cards);
            await _scryContext.SaveChangesAsync();
        }

        public async Task UpdateScryCard(ScryfallCard card)
        {
            if (card.Id < 1)
            {
                throw new ArgumentException("Invalid card Id");
            }
            _scryContext.Cards.Update(card);
            await _scryContext.SaveChangesAsync();
        }

        public async Task<ScryfallCard> GetScryCardById(int multiverseId)
        {
            var result = await _scryContext.Cards.FirstOrDefaultAsync(x => x.MultiverseId == multiverseId);
            return result;
        }

        public async Task AddScrySet(ScryfallSet set)
        {
            if (set.Id > 0)
            {
                throw new ArgumentException("Invalid card Id");
            }
            await _scryContext.Sets.AddAsync(set);
            await _scryContext.SaveChangesAsync();
        }

        public async Task UpdateScrySet(ScryfallSet set)
        {
            if (set.Id < 1)
            {
                throw new ArgumentException("Invalid card Id");
            }
            _scryContext.Sets.Update(set);
            await _scryContext.SaveChangesAsync();
        }

        public async Task<ScryfallSet> GetScrySetByCode(string setCode)
        {
            var result = await _scryContext.Sets.FirstOrDefaultAsync(x => x.Code == setCode);
            return result;
        }

        public IQueryable<ScryfallSet> QueryScrySets()
        {
            IQueryable<ScryfallSet> result = _scryContext.Sets.AsQueryable();
            return result;
        }

        #endregion

        #region private members

        public List<ScryfallMagicCard> MapScryfallDataToCards(List<JToken> cardSearchData)
        {
            try
            {


                //_logger.LogWarning("Begin MapScryfallDataToCards");

                List<ScryfallMagicCard> updatedCards = new List<ScryfallMagicCard>();
                List<JToken> specialCards = new List<JToken>();

                //for each card
                cardSearchData.ForEach(card =>
                {
                    try
                    {
                        //does it have at least 1 MID?
                        int? parsedMID = (int?)card.SelectToken("multiverse_ids[0]");

                        if (parsedMID != null)
                        {
                            ScryfallMagicCard cardToAdd = new ScryfallMagicCard();
                            cardToAdd.RefreshFromToken(card);

                            updatedCards.Add(cardToAdd);

                        }
                        else
                        {
                            specialCards.Add(card);
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                });

                //a 'special card' is a unique variation of a named card already in the set
                specialCards.ForEach(specialCard =>
                {

                    try
                    {
                        string cardName = specialCard.Value<string>("name");
                        var cardToUpdate = updatedCards.Where(x => x.Name == cardName).FirstOrDefault(); //should this just be First()?
                        if (cardToUpdate != null)
                        {
                            //_logger.LogWarning($"Applying variant to {cardName}");

                            cardToUpdate.ApplyVariant(specialCard);
                        }
                        else
                        {
                            //_logger.LogError($"Could not find matching card for special card: {cardName}");
                        }

                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                });

                //_logger.LogWarning("Completed MapScryfallDataToCards");



                return updatedCards;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> EnsureSetExistsLocally(string setCode)
        {
            try
            {
                await ForceUpdateScryfallSet(setCode);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task ForceUpdateScryfallSet(string setCode)
        {

            var thisSet = _scryContext.Sets.Where(x => x.Code == setCode).FirstOrDefault();

            if (thisSet == null)
            {
                ScryfallSet newSet = new ScryfallSet()
                {
                    Code = setCode,
                    LastUpdated = null
                };

                _scryContext.Sets.Add(newSet);
                thisSet = newSet;
            }

            //Get the set data from scryfall
            ScryfallSetDataDto cardSearchData = await RequestFullScryfallSet(setCode);

            //make sure Last Updated is up to date
            thisSet.LastUpdated = DateTime.Now.Date;

            #region Save everything to the DB

            cardSearchData.Cards.ForEach(item =>
            {
                try
                {
                    var storedCard = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == item.MultiverseId);
                    if (storedCard != null)
                    {
                        storedCard.StringData = item.Serialize();
                        //_scryContext.Update(storedCard);
                    }
                    else
                    {
                        var cardToAdd = new ScryfallCard()
                        {
                            MultiverseId = item.MultiverseId,
                            StringData = item.Serialize(),
                            Set = thisSet
                        };
                        _scryContext.Cards.Add(cardToAdd);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
            //_scryContext.Sets.Update(thisSet);

            //do I need to be calling .Update() on everything??
            await _scryContext.SaveChangesAsync();

            #endregion

        }

        public async Task<ScryfallSetDataDto> RequestFullScryfallSet(string setCode)
        {
            ScryfallSetDataDto result = new ScryfallSetDataDto();

            List<JToken> rawCardResults = new List<JToken>();

            var setEndpoint = $"https://api.scryfall.com/sets/{setCode}";
            var setResponseString = await _client.GetStringAsync(setEndpoint);

            JObject setResponseObject = JObject.Parse(setResponseString);

            bool searchHasMore = true;

            string cardSearchUri = setResponseObject.Value<string>("search_uri");

            result.Name = setResponseObject.Value<string>("name");

            result.Code = setResponseObject.Value<string>("code");

            result.ReleaseDate = setResponseObject.Value<string>("released_at");

            while (searchHasMore)
            {
                await Task.Delay(_scryfallApiDelay);
                var cardSearchResponse = await _client.GetStringAsync(cardSearchUri);
                JObject cardSearchJobject = JObject.Parse(cardSearchResponse);

                JArray dataBatch = cardSearchJobject.Value<JArray>("data");

                rawCardResults.AddRange(dataBatch);
                searchHasMore = cardSearchJobject.Value<bool>("has_more");
                if (searchHasMore)
                {
                    cardSearchUri = cardSearchJobject.Value<string>("next_page");
                }
            }

            result.Cards = MapScryfallDataToCards(rawCardResults);

            return result;
        }

        private async Task<ScryfallMagicCard> QueryScryfallById(int multiverseId)
        {
            //What does SET SEARCH return?
            string endpoint = $"https://api.scryfall.com/cards/multiverse/{multiverseId}";

            var responseString = await _client.GetStringAsync(endpoint);

            JObject responseObject = JObject.Parse(responseString);

            ScryfallMagicCard cardResult = new ScryfallMagicCard();
            cardResult.RefreshFromToken(responseObject);

            return cardResult;
        }

        //WARNING - the ScryfallMagicCard returned by this won't have all variant information
        private async Task<ScryfallMagicCard> QueryScryfallBySetName(string name, string setCode)
        {
            string endpoint = $"https://api.scryfall.com/cards/named?exact={name}&set={setCode}";

            var responseString = await _client.GetStringAsync(endpoint);

            JObject responseObject = JObject.Parse(responseString);

            ScryfallMagicCard cardResult = new ScryfallMagicCard();

            cardResult.RefreshFromToken(responseObject);

            return cardResult;
        }
        #endregion
    }
}



namespace Sqlite.Implementations
{
    public class ScryfallRepo //: ICardStringRepo
    {
        private readonly ILogger<ScryfallRepo> _logger;
        private readonly HttpClient _client;
        private readonly ScryfallDataContext _scryContext;
        private readonly int _scryfallApiDelay;

        public ScryfallRepo(
            ILogger<ScryfallRepo> logger,
            HttpClient client,
            ScryfallDataContext scryContext
            )
        {
            _logger = logger;
            _client = client;
            _scryContext = scryContext;
            _scryfallApiDelay = 1000; //1 second
        }

        #region public members

        public async Task EnsureDatabaseCreated()
        {
            await _scryContext.Database.EnsureCreatedAsync();
        }

        public async Task<IQueryable<ScryfallMagicCard>> QueryCardsBySet(string setCode)
        {
            var allCardsQuery = await _scryContext.Cards.Where(x => x.Set.Code != null && x.Set.Code.ToLower() == setCode.ToLower()).ToListAsync();

            var query = allCardsQuery.Select(x => JObject.Parse(x.StringData).ToObject<ScryfallMagicCard>());

            return query.AsQueryable();
        }

        public async Task<IQueryable<ScryfallMagicCard>> QueryScryfallByName(string name, bool exclusive)
        {
            //What does SET SEARCH return?
            string endpoint;

            string nameParam = name.Replace(' ', '+');

            if (exclusive)
            {
                endpoint = $"https://api.scryfall.com/cards/search?q=!%22{nameParam}%22&unique=prints";
            }
            else
            {
                endpoint = $"https://api.scryfall.com/cards/search?q={nameParam}";
            }

            var responseString = await _client.GetStringAsync(endpoint);

            JObject responseObject = JObject.Parse(responseString);
            var cardResultData = responseObject.Value<JArray>("data").ToList();

            List<ScryfallMagicCard> mappedCards = MapScryfallDataToCards(cardResultData);

            return mappedCards.AsQueryable();
        }

        public async Task<ScryfallMagicCard> GetCardById(int multiverseId)
        {
            var cardInDb = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == multiverseId);

            if (cardInDb == null)
            {
                //Okay, if this card doesn't exist, I can't trust a query by MID (won't have all variants)
                //BUT, I can use the MID to find the set, THEN call EnsureSetExists, then return the card (wow)

                var rawScryfallCard = await QueryScryfallById(multiverseId);

                await EnsureSetExistsLocally(rawScryfallCard.Set);

                cardInDb = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == multiverseId);
            }

            var cardData = JObject.Parse(cardInDb.StringData).ToObject<ScryfallMagicCard>();

            return cardData;
        }

        public async Task<int> GetCardMultiverseId(string name, string setCode)
        {
            var card = await QueryScryfallBySetName(name, setCode);
            return card.MultiverseId;
        }




        public async Task AddScryCard(ScryfallCard card)
        {
            if (card.Id > 0)
            {
                throw new ArgumentException("Invalid card Id");
            }
            await _scryContext.Cards.AddAsync(card);
            await _scryContext.SaveChangesAsync();
        }

        public async Task AddScryCards(List<ScryfallCard> cards)
        {
            //if (card.Id > 0)
            //{
            //    throw new ArgumentException("Invalid card Id");
            //}
            await _scryContext.AddRangeAsync(cards);
            await _scryContext.SaveChangesAsync();
        }

        public async Task UpdateScryCard(ScryfallCard card)
        {
            if (card.Id < 1)
            {
                throw new ArgumentException("Invalid card Id");
            }
            _scryContext.Cards.Update(card);
            await _scryContext.SaveChangesAsync();
        }

        public async Task<ScryfallCard> GetScryCardById(int multiverseId)
        {
            var result = await _scryContext.Cards.FirstOrDefaultAsync(x => x.MultiverseId == multiverseId);
            return result;
        }

        public async Task AddScrySet(ScryfallSet set)
        {
            if (set.Id > 0)
            {
                throw new ArgumentException("Invalid card Id");
            }
            await _scryContext.Sets.AddAsync(set);
            await _scryContext.SaveChangesAsync();
        }

        public async Task UpdateScrySet(ScryfallSet set)
        {
            if (set.Id < 1)
            {
                throw new ArgumentException("Invalid card Id");
            }
            _scryContext.Sets.Update(set);
            await _scryContext.SaveChangesAsync();
        }

        public async Task<ScryfallSet> GetScrySetByCode(string setCode)
        {
            var result = await _scryContext.Sets.FirstOrDefaultAsync(x => x.Code == setCode);
            return result;
        }

        public IQueryable<ScryfallSet> QueryScrySets()
        {
            IQueryable<ScryfallSet> result = _scryContext.Sets.AsQueryable();
            return result;
        }

        #endregion

        #region private members

        private List<ScryfallMagicCard> MapScryfallDataToCards(List<JToken> cardSearchData)
        {
            try
            {


                //_logger.LogWarning("Begin MapScryfallDataToCards");

                List<ScryfallMagicCard> updatedCards = new List<ScryfallMagicCard>();
                List<JToken> specialCards = new List<JToken>();

                //for each card
                cardSearchData.ForEach(card =>
                {
                    try
                    {
                        //does it have at least 1 MID?
                        int? parsedMID = (int?)card.SelectToken("multiverse_ids[0]");

                        if (parsedMID != null)
                        {
                            ScryfallMagicCard cardToAdd = new ScryfallMagicCard();
                            cardToAdd.RefreshFromToken(card);

                            updatedCards.Add(cardToAdd);

                        }
                        else
                        {
                            specialCards.Add(card);
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                });

                //a 'special card' is a unique variation of a named card already in the set
                specialCards.ForEach(specialCard =>
                {

                    try
                    {
                        string cardName = specialCard.Value<string>("name");
                        var cardToUpdate = updatedCards.Where(x => x.Name == cardName).FirstOrDefault(); //should this just be First()?
                        if (cardToUpdate != null)
                        {
                            //_logger.LogWarning($"Applying variant to {cardName}");

                            cardToUpdate.ApplyVariant(specialCard);
                        }
                        else
                        {
                            //_logger.LogError($"Could not find matching card for special card: {cardName}");
                        }

                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                });

                //_logger.LogWarning("Completed MapScryfallDataToCards");



                return updatedCards;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> EnsureSetExistsLocally(string setCode)
        {
            try
            {
                await ForceUpdateScryfallSet(setCode);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task ForceUpdateScryfallSet(string setCode)
        {

            var thisSet = _scryContext.Sets.Where(x => x.Code == setCode).FirstOrDefault();

            if (thisSet == null)
            {
                ScryfallSet newSet = new ScryfallSet()
                {
                    Code = setCode,
                    LastUpdated = null
                };

                _scryContext.Sets.Add(newSet);
                thisSet = newSet;
            }

            //Get the set data from scryfall
            ScryfallSetDataDto cardSearchData = await RequestFullScryfallSet(setCode);

            //make sure Last Updated is up to date
            thisSet.LastUpdated = DateTime.Now.Date;

            #region Save everything to the DB

            cardSearchData.Cards.ForEach(item =>
            {
                try
                {
                    var storedCard = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == item.MultiverseId);
                    if (storedCard != null)
                    {
                        storedCard.StringData = item.Serialize();
                        //_scryContext.Update(storedCard);
                    }
                    else
                    {
                        var cardToAdd = new ScryfallCard()
                        {
                            MultiverseId = item.MultiverseId,
                            StringData = item.Serialize(),
                            Set = thisSet
                        };
                        _scryContext.Cards.Add(cardToAdd);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
            //_scryContext.Sets.Update(thisSet);

            //do I need to be calling .Update() on everything??
            await _scryContext.SaveChangesAsync();

            #endregion

        }

        public async Task<ScryfallSetDataDto> RequestFullScryfallSet(string setCode)
        {
            ScryfallSetDataDto result = new ScryfallSetDataDto();

            List<JToken> rawCardResults = new List<JToken>();

            var setEndpoint = $"https://api.scryfall.com/sets/{setCode}";
            var setResponseString = await _client.GetStringAsync(setEndpoint);

            JObject setResponseObject = JObject.Parse(setResponseString);

            bool searchHasMore = true;

            string cardSearchUri = setResponseObject.Value<string>("search_uri");

            result.Name = setResponseObject.Value<string>("name");

            result.Code = setResponseObject.Value<string>("code");

            result.ReleaseDate = setResponseObject.Value<string>("released_at");

            while (searchHasMore)
            {
                await Task.Delay(_scryfallApiDelay);
                var cardSearchResponse = await _client.GetStringAsync(cardSearchUri);
                JObject cardSearchJobject = JObject.Parse(cardSearchResponse);

                JArray dataBatch = cardSearchJobject.Value<JArray>("data");

                rawCardResults.AddRange(dataBatch);
                searchHasMore = cardSearchJobject.Value<bool>("has_more");
                if (searchHasMore)
                {
                    cardSearchUri = cardSearchJobject.Value<string>("next_page");
                }
            }

            result.Cards = MapScryfallDataToCards(rawCardResults);

            return result;
        }

        private async Task<ScryfallMagicCard> QueryScryfallById(int multiverseId)
        {
            //What does SET SEARCH return?
            string endpoint = $"https://api.scryfall.com/cards/multiverse/{multiverseId}";

            var responseString = await _client.GetStringAsync(endpoint);

            JObject responseObject = JObject.Parse(responseString);

            ScryfallMagicCard cardResult = new ScryfallMagicCard();
            cardResult.RefreshFromToken(responseObject);

            return cardResult;
        }

        //WARNING - the ScryfallMagicCard returned by this won't have all variant information
        private async Task<ScryfallMagicCard> QueryScryfallBySetName(string name, string setCode)
        {
            string endpoint = $"https://api.scryfall.com/cards/named?exact={name}&set={setCode}";

            var responseString = await _client.GetStringAsync(endpoint);

            JObject responseObject = JObject.Parse(responseString);

            ScryfallMagicCard cardResult = new ScryfallMagicCard();

            cardResult.RefreshFromToken(responseObject);

            return cardResult;
        }
        #endregion
    }
}
