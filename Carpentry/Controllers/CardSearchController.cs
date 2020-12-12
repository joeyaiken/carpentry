using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Search;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardSearchController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Card Search controller: {ex.Message}";
        }

        private readonly ISearchService _searchService;
        private readonly IScryfallService _scryService;

        public CardSearchController(ISearchService searchService, IScryfallService scryService)
        {
            _searchService = searchService;
            _scryService = scryService;
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
                var cards = await _searchService.SearchCardDefinitions(filters);
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
            //TODO - swap return object for CardSearchResultDto
            try
            {
                var result = await _scryService.SearchScryfallByName(param.Name, param.Exclusive);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchWeb", ex));
            }
        }

        #endregion Search Methods
    }
}
