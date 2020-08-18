using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Search;
using Carpentry.Service.Interfaces;
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
        public async Task<ActionResult<List<CardSearchResultDto>>> SearchInventory([FromBody] CardSearchQueryParameter filters)
        {
            try
            {
                var cards = await _cardSearch.SearchInventory(filters);
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
    }
}
