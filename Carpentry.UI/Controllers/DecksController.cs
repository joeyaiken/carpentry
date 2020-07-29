using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
//using Carpentry.Service.Interfaces;
//using Carpentry.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Controllers
{
    [Route("api/[controller]")]
    public class DecksController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Decks controller: {ex.Message}";
        }

        private readonly IDeckService _decks;

        public DecksController(IDeckService decks)
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

        //decks/add
        //- add a deck
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

        //decks/update
        //- update properties of a deck
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

        //decks/delete
        //- delete a deck
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
        public async Task<ActionResult> AddDeckCard([FromBody] DeckCardDto dto)
        {
            try
            {
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

        //decks/Search
        //- get a list of deck properties & stats
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

        //decks/Get
        //- get a deck (with cards)
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
        public async Task<ActionResult<ValidatedDeckImportDto>> ValidateDeckImport(DeckImportDto dto)
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

        //ExportDeckList
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
