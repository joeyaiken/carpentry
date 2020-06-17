using Carpentry.Logic.Interfaces;
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
            _scryfallApiDelay = 200; //1000 = 1 second, scryfall requests 50-100
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

        public async Task<ScryfallSetDataDto> GetFullSet(string setCode)
        {
            ScryfallSetDataDto result = new ScryfallSetDataDto()
            {
                CardTokens = new List<JToken>(),
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
                            int breakpoint = 1;
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

        public async Task<List<ScryfallMagicCard>> SearchScryfallByName(string name, bool exclusive)
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

            return mappedCards;
        }

    }
}
