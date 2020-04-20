using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Carpentry.Data.Models;
//using Carpentry.Data.Models;
//using Carpentry.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.UI.Legacy.Models;
using Carpentry.UI.Legacy.Util;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Legacy.Controllers
{
    [Route("api/[controller]")]
    public class DecksController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Decks controller: {ex.Message}";
        }

        private readonly IDeckService _decks;
        private readonly IMapperService _mapper;

        public DecksController(IDeckService decks, IMapperService mapper)
        {
            _decks = decks;
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

        //decks/add
        //- add a deck
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> Add([FromBody] LegacyDeckPropertiesDto deckProps)
        {
            try
            {
                int newDeckId = await _decks.AddDeck(await _mapper.ToModel(deckProps));
                return Ok(newDeckId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Add", ex));
            }
        }

        //decks/update
        //- update properties of a deck
        [HttpPost("[action]")]
        public async Task<ActionResult> Update([FromBody] LegacyDeckPropertiesDto deckProps)
        {
            try
            {
                await _decks.UpdateDeck(await _mapper.ToModel(deckProps));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Update", ex));
            }
        }

        //decks/delete
        //- delete a deck
        [HttpGet("[action]")]
        public async Task<ActionResult> Delete(int deckId)
        {
            try
            {
                await _decks.DeleteDeck(deckId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Delete", ex));
            }
        }

        //decks/Search
        //- get a list of deck properties & stats
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<LegacyDeckPropertiesDto>>> Search()
        {
            try
            {
                var results = await _decks.GetDeckOverviews();
                IEnumerable<LegacyDeckPropertiesDto> mappedResults = await _mapper.ToDto(results);
                return Ok(mappedResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Search", ex));
            }
        }

        //decks/Get
        //- get a deck (with cards)
        [HttpGet("[action]")]
        public async Task<ActionResult<LegacyDeckDetailDto>> Get(int deckId)
        {
            try
            {
                var results = await _decks.GetDeckDetail(deckId);
                LegacyDeckDetailDto mappedResults = await _mapper.ToDto(results);
                return Ok(mappedResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Get", ex));
            }
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> AddCard([FromBody] LegacyDeckCardDto dto)
        {
            try
            {
                var model = _mapper.ToModel(dto);
                await _decks.AddDeckCard(model);
                //await _decks.AddDeckCard(dto.ToModel());
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddCard", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateCard([FromBody] LegacyDeckCardDto card)
        {
            try
            {
                var cardModel = _mapper.ToModel(card);
                await _decks.UpdateDeckCard(cardModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("UpdateCard", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> RemoveCard(int id)
        {
            try
            {
                await _decks.DeleteDeckCard(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("RemoveCard", ex));
            }
        }
    }
}
