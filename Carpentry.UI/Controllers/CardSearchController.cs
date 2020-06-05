using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Carpentry.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardSearchController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Card Search controller: {ex.Message}";
        }

        private readonly ICardSearchService _cardSearch;

        public CardSearchController(ICardSearchService cardSearch)
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
                IEnumerable<MagicCardDto> cards = await _cardSearch.SearchCardsFromWeb(param);
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchWeb", ex));
            }
        }

        /// <summary>
        /// Searches all cards in a given set.
        /// In earlier versions, this involved searching the cached scryfall repo, but now we can just search the regular card repo
        /// This could / should probably be merged with SearchInventory
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        //public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] CardSearchQueryParameter filters)
        public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] CardSearchQueryParameter filters)
        //public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] JsonObj filters)
        //public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] JObject filters)
        {
            try
            {
                //CardSearchQueryParameter newFilters = new CardSearchQueryParameter();
                IEnumerable<MagicCardDto> cards = await _cardSearch.SearchCardsFromSet(filters);
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchSet", ex));
            }
        }

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
                IEnumerable<MagicCardDto> cards = await _cardSearch.SearchCardsFromInventory(filters);
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchInventory", ex));
            }
        }
    }
}
