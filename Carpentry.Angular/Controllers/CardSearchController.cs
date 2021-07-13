using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Search;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.Angular.Controllers
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

        public CardSearchController(ISearchService searchService)
        {
            _searchService = searchService;
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

        //What if this replaced the Inventory Overview search?
        //Everything that needs a list of cards could use this controller
        //  inventory overviews (search by name/print/unique)
        //  inventory add cards (search by print, group by name?... actuyally IDK how that should go)
        //  deck add cards (search by name)
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

        #endregion Search Methods
    }
}
