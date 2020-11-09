using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic.Models;
using Carpentry.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Carpentry.Controllers
{
    [Route("api/[controller]")]
    public class DecksController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Decks controller: {ex.Message}";
        }

        private readonly ICarpentryDeckService _decks;

        public DecksController(ICarpentryDeckService decks)
        {
            _decks = decks;
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

        #region Deck Props

        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddDeck([FromBody] DeckPropertiesDto deckProps)
        {
            try
            {
                int newDeckId = await _decks.AddDeck(deckProps);
                return Ok(newDeckId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Add", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateDeck([FromBody] DeckPropertiesDto deckProps)
        {
            try
            {
                await _decks.UpdateDeck(deckProps);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Update", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> DeleteDeck(int deckId)
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

        #endregion Deck Props

        #region Deck Cards

        [HttpPost("[action]")]
        //public async Task<ActionResult> AddDeckCard([FromBody] object rawDto)
        public async Task<ActionResult> AddDeckCard([FromBody] DeckCardDto dto)
        {
            //int breakpoint = 1;
            //return Ok();
            try
            {


                //DeckCardDto dto = (DeckCardDto)rawDto;
                await _decks.AddDeckCard(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddCard", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateDeckCard([FromBody] DeckCardDto card)
        {
            try
            {
                await _decks.UpdateDeckCard(card);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("UpdateCard", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> RemoveDeckCard(int id)
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

        #endregion Deck Cards

        #region Search

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<DeckOverviewDto>>> GetDeckOverviews()
        {
            try
            {
                IEnumerable<DeckOverviewDto> results = await _decks.GetDeckOverviews();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Search", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<DeckDetailDto>> GetDeckDetail(int deckId)
        {
            try
            {
                DeckDetailDto results = await _decks.GetDeckDetail(deckId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Get", ex));
            }
        }

        #endregion Search

        #region Import

        [HttpPost("[action]")]
        public async Task<ActionResult<ValidatedDeckImportDto>> ValidateDeckImport(CardImportDto dto)
        {
            try
            {
                ValidatedDeckImportDto results = await _decks.ValidateDeckImport(dto);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ValidateDeckImport", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AddValidatedDeckImport(ValidatedDeckImportDto dto)
        {
            try
            {
                await _decks.AddValidatedDeckImport(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ValidateDeckImport", ex));
            }
        }

        #endregion Import

        #region Export

        [HttpGet("[action]")]
        public async Task<ActionResult<string>> ExportDeckList(int deckId)
        {
            try
            {
                string results = await _decks.ExportDeckList(deckId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ValidateDeckImport", ex));
            }
        }

        #endregion Export

    }
}
