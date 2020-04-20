using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
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
        public IActionResult Get()
        {
            return Ok("Online");
        }

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

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchSet([FromBody] CardSearchQueryParameter filters)
        {
            try
            {
                IEnumerable<MagicCardDto> cards = await _cardSearch.SearchCardsFromSet(filters);
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchSet", ex));
            }
        }

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
