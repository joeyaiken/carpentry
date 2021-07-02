using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Scryfall;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class ScryfallService : IScryfallService
    {
        //private readonly ILogger<ScryfallRepo> _logger;
        private readonly HttpClient _client;
        private readonly int _scryfallApiDelay;

        public ScryfallService(
            //ILogger<ScryfallRepo> logger,
            HttpClient client
        )
        {
            _client = client;

            //TODO - load this from a config
            _scryfallApiDelay = 100; //1000 = 1 second, scryfall requests 50-100
        }

        /// <summary>
        /// Keep in mind this doesn't return variants 
        /// </summary>
        /// <param name="multiverseId"></param>
        /// <returns></returns>
        public async Task<ScryfallMagicCard> GetCardByMid(int multiverseId)
        {
            string endpoint = $"https://api.scryfall.com/cards/multiverse/{multiverseId}";

            var responseString = await _client.GetStringAsync(endpoint);

            JObject responseObject = JObject.Parse(responseString);

            ScryfallMagicCard cardResult = new ScryfallMagicCard();
            cardResult.RefreshFromToken(responseObject);

            return cardResult;
        }

        /// <summary>
        /// Gets a full set of Card Tokens from Scryfall
        /// Does not do any mapping (mapped lists are null)
        /// </summary>
        /// <param name="setCode"></param>
        /// <returns></returns>
        public async Task<ScryfallSetDataDto> GetFullSet(string setCode)
        {
            ScryfallSetDataDto result = new ScryfallSetDataDto()
            {
                CardTokens = new List<JToken>(),
                //SetCards = new List<ScryfallMagicCard>(),
                //PremiumCards = new List<ScryfallMagicCard>(),
            };

            //List<JToken> rawCardResults = new List<JToken>();

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

                result.CardTokens.AddRange(dataBatch);
                searchHasMore = cardSearchJobject.Value<bool>("has_more");
                if (searchHasMore)
                {
                    cardSearchUri = cardSearchJobject.Value<string>("next_page");
                }
            }
            
            //result.Cards = MapScryfallDataToCards(rawCardResults);

            return result;
        }

        public async Task<ScryfallSetDataDto> GetFullMappedSet(string setCode)
        {
            var rawSet = await GetFullSet(setCode);

            //Should this be a try/catch?
            var mappedSet = MapScryfallSetData(rawSet);

            return mappedSet;
        }

        /// <summary>
        /// Takes a ScryfallSetDataDto and maps Card Tokens to ScryfallMagicCard objects
        /// </summary>
        /// <param name="setData"></param>
        /// <returns></returns>
        public ScryfallSetDataDto MapScryfallSetData(ScryfallSetDataDto setData)
        {
            //clear what's ever set for mapped cards
            setData.SetCards = new List<ScryfallMagicCard>();
            setData.PremiumCards = new List<ScryfallMagicCard>();

            //Parse each token & add to a list
            foreach(var card in setData.CardTokens)
            {
                try
                {
                    ScryfallMagicCard cardToAdd = new ScryfallMagicCard();
                    cardToAdd.RefreshFromToken(card);

                    if (cardToAdd.DoNotAdd)
                    {
                        continue;
                    }

                    if (cardToAdd.IsPremium)
                    {
                        setData.PremiumCards.Add(cardToAdd);
                    }
                    else
                    {
                        setData.SetCards.Add(cardToAdd);
                    }
                }
                //Hopefully I won't hit this, but I'm leaving it in for DEV
                catch(Exception ex)
                {
                    throw new Exception($"Issue mapping scryfall card: {ex.Message}");
                }
            }

            return setData;
        }

        [Obsolete]
        public List<ScryfallMagicCard> MapScryfallDataToCards(List<JToken> cardSearchData)
        {
            try
            {

                //★

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

    //                        cardToUpdate.ApplyVariant(specialCard);
                        }
                        else
                        {
                            int breakpoint = 1;
                            //So this isn't getting M21 - RinAndSeri properly...
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

        //This will return the (filtered) list of sets from scryfall
        //It won't include, for example, anything online-only
        //Maybe in the future, include a param to include promos/whatnot
        public async Task<List<ScryfallSetOverviewDto>> GetAllSets()
        {
            var endpoint = $"https://api.scryfall.com/sets";
            var responseString = await _client.GetStringAsync(endpoint);

            JObject setResponseObject = JObject.Parse(responseString);

            JArray responseData = setResponseObject.Value<JArray>("data");

            List<ScryfallSetOverviewDto> result = new List<ScryfallSetOverviewDto>();

            foreach(var scrySet in responseData)
            {
                //need JObject not JToken



                //result.Code = setResponseObject.Value<string>("code");

                var newDto = new ScryfallSetOverviewDto()
                {
                    //"object": "set",
                    //"id": "372dafe8-b5d1-4b81-998f-3ae96b59498a",
                    Code = scrySet.Value<string>("code"),
                    //"mtgo_code": "2xm",
                    //"arena_code": "2xm",
                    //"tcgplayer_id": 2655,
                    Name = scrySet.Value<string>("name"),
                    //"uri": "https://api.scryfall.com/sets/372dafe8-b5d1-4b81-998f-3ae96b59498a",
                    //"scryfall_uri": "https://scryfall.com/sets/2xm",
                    //"search_uri": "https://api.scryfall.com/cards/search?order=set&q=e%3A2xm&unique=prints",
                    ReleasedAtString = scrySet.Value<string>("released_at"),
                    SetType = scrySet.Value<string>("set_type"),
                    CardCount = scrySet.Value<int>("card_count"),
                    Digital = scrySet.Value<bool>("digital"),
                    NonfoilOnly = scrySet.Value<bool>("nonfoil_only"),
                    FoilOnly = scrySet.Value<bool>("foil_only"),
                    //"icon_svg_uri": "https://img.scryfall.com/sets/2xm.svg?1594612800"
                };

                result.Add(newDto);
            }

            //funny
            string[] excludedSetTypes = {
                "token",
                "funny",
                //"memorabilia",//If I want to see the c21 display commanders I need to include this
                "promo",
                "box",
            };

            result = result
                .Where(s => 
                    s.Digital == false
                        &&
                    !excludedSetTypes.Contains(s.SetType)
                        &&
                    !s.Name.Contains("Judge ")
                )
                .ToList();
            //This should be filtered

            return result;
        }

    }
}
