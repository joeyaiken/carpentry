using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Carpentry.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardSearchController : ControllerBase
    {

        //      

        /*  This controller should just have 2 methods
                Search Web / Search Scryfall, a method that calls the scryfall search API
                    Currently this only searches by name, and has an override for all prints
                Search Inventory, a method that queries the local DB
                    Contains filters for: 
                        Only owned cards
                        Card data filters
            */

        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Card Search controller: {ex.Message}";
        }

        private readonly ICarpentryCardSearchService _cardSearch;

        public CardSearchController(ICarpentryCardSearchService cardSearch)
        {
            _cardSearch = cardSearch;
        }

        /// <summary>
        /// This method just ensures the controller can start correctly (catches DI issues)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok("Online");
        }

        #region Search Methods

        /// <summary>
        /// Searches cards in the inventory
        /// This could / should probably be merged with SearchSet
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchInventory([FromBody] InventoryQueryParameter filters)
        {
            try
            {
                IEnumerable<MagicCardDto> cards = await _cardSearch.SearchInventory(filters);
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchInventory", ex));
            }
        }

        /// <summary>
        /// Will call the scryfall API to get cards by name, returning mapped results
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchWeb([FromBody] NameSearchQueryParameter param)
        {
            try
            {
                IEnumerable<MagicCardDto> cards = await _cardSearch.SearchWeb(param);
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchWeb", ex));
            }
        }

        #endregion Search Methods

        #region Obsolete 

        ///// <summary>
        ///// Searches all cards in a given set.
        ///// In earlier versions, this involved searching the cached scryfall repo, but now we can just search the regular card repo
        ///// This could / should probably be merged with SearchInventory
        ///// </summary>
        ///// <param name="filters"></param>
        ///// <returns></returns>
        //[HttpPost("[action]")]
        ////public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] CardSearchQueryParameter filters)
        //public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] CardSearchQueryParameter filters)
        ////public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] JsonObj filters)
        ////public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] JObject filters)
        //{
        //    try
        //    {
        //        //CardSearchQueryParameter newFilters = new CardSearchQueryParameter();
        //        IEnumerable<MagicCardDto> cards = await _cardSearch.SearchCardsFromSet(filters);
        //        return Ok(cards);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("SearchSet", ex));
        //    }
        //}

        #endregion
    }
}
