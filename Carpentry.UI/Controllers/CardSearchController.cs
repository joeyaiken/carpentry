using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        private readonly ICarpentryService _carpentry;

        public CardSearchController(ICarpentryService carpentry)
        {
            _carpentry = carpentry;
        }

        /// <summary>
        /// This method just ensures the controller can start correctly (catches DI issues)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Online");
        }

        //[HttpPost("[action]")]
        //public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchWeb([FromBody] NameSearchQueryParameter param)
        //{
        //    try
        //    {
        //        var cards = await _carpentry.SearchCardsFromWeb(param);
        //        return Ok(cards);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("SearchWeb", ex));
        //    }
        //}

        //[HttpPost("[action]")]
        //public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] CardSearchQueryParameter filters)
        //{
        //    try
        //    {
        //        var cards = await _carpentry.SearchCardsFromSet(filters);
        //        return Ok(cards);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("SearchSet", ex));
        //    }
        //}

        //[HttpPost("[action]")]
        //public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchInventory([FromBody] InventoryQueryParameter filters)
        //{
        //    try
        //    {
        //        var cards = await _carpentry.SearchCardsFromInventory(filters);
        //        return Ok(cards);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("SearchInventory", ex));
        //    }
        //}
    }
}
