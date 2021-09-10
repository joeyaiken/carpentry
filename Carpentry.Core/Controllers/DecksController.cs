using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.Core.Controllers
{
    [Route("api/[controller]")]
    public class DecksController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Decks controller: {ex.Message}";
        }

        private readonly IDeckService _deckService;
        private readonly IDataImportService _cardImportService;

        public DecksController(IDeckService deckService, IDataImportService cardImportService)
        {
            _deckService = deckService;
            _cardImportService = cardImportService;
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

        #region Deck CRUD

        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddDeck([FromBody] DeckPropertiesDto deckProps)
        {
            try
            {
                int newDeckId = await _deckService.AddDeck(deckProps);
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
            if (deckProps == null)
            {
                return StatusCode(500, "Deck Properties null, cannot update deck");
            }
            try
            {
                await _deckService.UpdateDeck(deckProps);
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
                await _deckService.DeleteDeck(deckId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Delete", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<int>> CloneDeck(int deckId)
        {
            try
            {
                int clonedDeckId = await _deckService.CloneDeck(deckId);
                return Ok(clonedDeckId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("CloneDeck", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> DissassembleDeck(int deckId)
        {
            try
            {
                await _deckService.DissassembleDeck(deckId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("DissassembleDeck", ex));
            }
        }

        #endregion Deck

        #region Deck Cards CRUD

        [HttpPost("[action]")]
        public async Task<ActionResult> AddDeckCard([FromBody] DeckCardDto dto)
        {
            try
            {
                await _deckService.AddDeckCard(dto);
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
                await _deckService.UpdateDeckCard(card);
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
                await _deckService.DeleteDeckCard(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("RemoveCard", ex));
            }
        }

        #endregion Deck Cards

        #region Card Tags

        [HttpGet("[action]")]
        public async Task<ActionResult<CardTagDetailDto>> GetCardTagDetails(int deckId, int cardId)
        {
            try
            {
                //https://localhost:44333/decks/52?cardId=4006&show=tags
                var result = await _deckService.GetCardTagDetails(deckId, cardId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetCardTagDetails", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AddCardTag([FromBody] CardTagDto cardTag)
        {
            try
            {
                await _deckService.AddCardTag(cardTag);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddCardTag", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> RemoveCardTag(int cardTagId) //[FromHeader] 
        {
            try
            {
                await _deckService.RemoveCardTag(cardTagId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("RemoveCardTag", ex));
            }
        }

        #endregion

        #region Search

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<DeckOverviewDto>>> GetDeckOverviews(string format, string sortBy, bool includeDissasembled)
        {
            try
            {
                IEnumerable<DeckOverviewDto> results = await _deckService.GetDeckOverviews(format, sortBy, includeDissasembled);
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
                DeckDetailDto results = await _deckService.GetDeckDetail(deckId);
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
        public async Task<ActionResult<ValidatedDeckImportDto>> ValidateDeckImport([FromBody] CardImportDto dto)
        {
            try
            {
                ValidatedDeckImportDto results = await _cardImportService.ValidateDeckImport(dto);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ValidateDeckImport", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddValidatedDeckImport([FromBody] ValidatedDeckImportDto dto)
        {
            try
            {
                var newId = await _cardImportService.AddValidatedDeckImport(dto);
                return Ok(newId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ValidateDeckImport", ex));
            }
        }

        #endregion Import

        #region Export

        [HttpGet("[action]")]
        public async Task<ActionResult<string>> ExportDeckList(int deckId, string exportType)
        {
            try
            {
                string results = await _deckService.GetDeckListExport(deckId, exportType);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ExportDeckList", ex));
            }
        }

        #endregion Export
    }
}
