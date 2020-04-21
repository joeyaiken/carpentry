using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Carpentry.Data.Models;
//using Carpentry.Data.QueryParameters;
using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using Carpentry.UI.Legacy.Models;
using Carpentry.UI.Legacy.Util;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Legacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardSearchController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Card Search controller: {ex.Message}";
        }

        private readonly ICardSearchControllerService _cardSearch;
        private readonly MapperService _mapper;

        public CardSearchController(ICardSearchControllerService cardSearch, MapperService mapper)
        {
            _cardSearch = cardSearch;
            _mapper = mapper;
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
        public async Task<ActionResult<IEnumerable<LegacyMagicCardDto>>> SearchWeb([FromBody] NameSearchQueryParameter param)
        {
            try
            {
                var cards = await _cardSearch.SearchCardsFromWeb(param);
                List<LegacyMagicCardDto> mappedCards = _mapper.ToLegacy(cards);
                return Ok(mappedCards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchWeb", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<LegacyMagicCardDto>>> SearchSet([FromBody] CardSearchQueryParameter filters)
        {
            try
            {
                var cards = await _cardSearch.SearchCardsFromSet(filters);
                List<LegacyMagicCardDto> mappedCards = _mapper.ToLegacy(cards);
                return Ok(mappedCards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchSet", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<LegacyMagicCardDto>>> SearchInventory([FromBody] InventoryQueryParameter filters)
        {
            try
            {
                var cards = await _cardSearch.SearchCardsFromInventory(filters);
                List<LegacyMagicCardDto> mappedCards = _mapper.ToLegacy(cards);
                return Ok(mappedCards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchInventory", ex));
            }
        }
    }
}
