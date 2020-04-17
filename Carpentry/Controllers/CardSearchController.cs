﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
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

        private readonly ICardSearchService _cardSearch;
        private readonly IMapperService _mapper;

        public CardSearchController(ICardSearchService cardSearch, IMapperService mapper)
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
        public async Task<ActionResult<IEnumerable<MagicCardDto>>> SearchWeb([FromBody] NameSearchQueryParameter param)
        {
            try
            {
                var cards = await _cardSearch.SearchCardsFromWeb(param);
                List<MagicCardDto> mappedCards = _mapper.ToDto(cards);
                return Ok(mappedCards);
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
                var cards = await _cardSearch.SearchCardsFromSet(filters);
                List<MagicCardDto> mappedCards = _mapper.ToDto(cards);
                return Ok(mappedCards);
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
                var cards = await _cardSearch.SearchCardsFromInventory(filters);
                List<MagicCardDto> mappedCards = _mapper.ToDto(cards);
                return Ok(mappedCards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("SearchInventory", ex));
            }
        }
    }
}
