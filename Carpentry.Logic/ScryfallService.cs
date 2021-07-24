using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Scryfall;
using Carpentry.ScryfallData;
using Carpentry.ScryfallData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Carpentry.Logic
{
    /// <summary>
    /// Provides a list of available sets from Scryfall, and all cards of a given set
    /// </summary>
    public interface IScryfallService
    {
        /// <summary>
        /// Get a list of all sets available from Scryfall
        /// Optionally excludes things not actually used by the app
        /// Stores results in a local database to avoid re-querying Scryfall for a given record type more than once a day
        /// </summary>
        /// <returns></returns>
        Task<List<ScryfallSetOverview>> GetSetOverviews(bool filter = true);

        /// <summary>
        /// Gets a detail object containing the set overview, and all cards contained in the set
        /// </summary>
        /// <param name="setCode"></param>
        /// <returns></returns>
        Task<ScryfallSetDetail> GetSetDetail(string setCode);


        ///// <summary>
        ///// Takes a list of set codes and ensures the sets are updated locally
        ///// </summary>
        ///// <returns></returns>
        //Task EnsureSetsUpdated(List<string> setCodes);
        //Task EndureSets

        //Use cases are:
        //  Getting list of available sets (from app settings, or maybe from import)
        //  Getting all cards in a specific set (calling scryfall if necessary)
        //  Ensuring a list of sets are up to date: EnsureSetsUpdated | EnsureSets[ Updated | Cached | Stored | UpToDate ]



        //"ensure set is up to date" (string setCode);
        //"Ensure list of sets are up to date"
    }

    class SetApiResult
    {
        public SetApiResult()
        {
            CardTokens = new List<JToken>();
        }

        public JToken SetToken { get; set; }
        public List<JToken> CardTokens { get; set; }
    }

    /// <summary>
    /// Handles the querying & storing of scryfall set and card definitions
    /// Scryfall updates their card and pricing data daily. 
    /// In order to avoid re-querying the API too often, responses are stored locally in the ScryfallData database
    /// 
    /// Public options include: Getting a list of all potential sets, and getting all cards of a specific set
    /// </summary>
    public class ScryfallService : IScryfallService
    {
        private readonly ScryfallDataContext _scryContext;
        private readonly ILogger<ScryfallService> _logger;
        private readonly HttpClient _client;
        private readonly int _scryfallApiDelay;

        public ScryfallService(
            ScryfallDataContext scryContext,
            ILogger<ScryfallService> logger,
            HttpClient client
        )
        {
            _scryContext = scryContext;
            _logger = logger;
            _client = client;

            //TODO - load this from a config
            _scryfallApiDelay = 100; //1000 = 1 second, scryfall requests 50-100
        }

        /// <summary>
        /// Get a list of all sets available from Scryfall
        /// Optionally excludes things not actually used by the app
        /// Stores results in a local database to avoid re-querying Scryfall for a given record type more than once a day
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScryfallSetOverview>> GetSetOverviews(bool filter = true)
        {
            //Check current audit data
            var setData = await _scryContext.ScryfallAuditData.FirstOrDefaultAsync();
            JArray setTokens = null;

            //If stale or non-existant, re-query from Scryfall
            if (setData == null || setData.LastUpdated == null || setData.LastUpdated.Value.Date < DateTime.Today)
            {
                if (setData == null) setData = new ScryfallAuditData();

                //get API result
                setTokens = await ApiGetAvailableSets();

                setData.SetTokensString = JsonConvert.SerializeObject(setTokens);
                setData.LastUpdated = DateTime.Now;

                _scryContext.Update(setData);

                await _scryContext.SaveChangesAsync();
            }

            //Parse tokens in case the DB record was recent-enough
            if (setTokens == null) setTokens = JsonConvert.DeserializeObject<JArray>(setData.SetTokensString);
            
            //Map tokens to classes
            var result = setTokens.Select(token => new ScryfallSetOverview(token)).ToList();

            //Filter unwanted sets (only used in dev/testing so-far)
            if(filter)
            {
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
            }

            return result;
        }

        /// <summary>
        /// Gets a detail object containing the set overview, and all cards contained in the set
        /// </summary>
        /// <param name="setCode"></param>
        /// <returns></returns>
        public async Task<ScryfallSetDetail> GetSetDetail(string setCode)
        {
            //Get the existing DB record
            var dbSet = await _scryContext.Sets.SingleOrDefaultAsync(s => s.Code.ToLower() == setCode.ToLower());
            
            //If stale enough, get an updated set from the Scryfall API
            if (dbSet == null || dbSet.LastUpdated == null || dbSet.LastUpdated.Value.Date < DateTime.Today)
            {
                if (dbSet == null) dbSet = new ScryfallSetData();

                //Get the API result (set & cards)
                var apiResult = await ApiGetFullSet(setCode);

                //Update & Save the DB object
                dbSet.SetTokenString = JsonConvert.SerializeObject(apiResult.SetToken);
                dbSet.CardTokensString = JsonConvert.SerializeObject(apiResult.CardTokens);
                dbSet.LastUpdated = DateTime.Now;
                _scryContext.Update(dbSet);
                await _scryContext.SaveChangesAsync();
            }

            //Parse the DB object
            var result = new ScryfallSetDetail()
            {
                Overview = new ScryfallSetOverview(JsonConvert.DeserializeObject<JToken>(dbSet.SetTokenString)),
                Cards = JsonConvert.DeserializeObject<List<JToken>>(dbSet.CardTokensString)
                    .Select(token => new ScryfallMagicCard(token)).ToList()
            };

            //result.Cards.ForEach(card => card.RefreshFromToken()); //This is required until I finish refactoring

            return result;
        }

        #region API calls

        private async Task<JArray> ApiGetAvailableSets()
        {
            var endpoint = $"https://api.scryfall.com/sets";
            var responseString = await _client.GetStringAsync(endpoint);

            var setResponseObject = JObject.Parse(responseString);

            var responseData = setResponseObject.Value<JArray>("data");

            return responseData;
        }

        /// <summary>
        /// Gets a full set of unmapped Card Tokens from Scryfall
        /// </summary>
        /// <param name="setCode"></param>
        /// <returns></returns>
        private async Task<SetApiResult> ApiGetFullSet(string setCode)
        {
            var result = new SetApiResult();

            var setEndpoint = $"https://api.scryfall.com/sets/{setCode}";
            var setResponseString = await _client.GetStringAsync(setEndpoint);

            //result.SetToken = JObject.Parse(setResponseString);
            result.SetToken = JToken.Parse(setResponseString);



            bool searchHasMore = true;

            string cardSearchUri = result.SetToken.Value<string>("search_uri");

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

            return result;
        }

        #endregion
    }
}
