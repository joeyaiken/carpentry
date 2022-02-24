using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Carpentry.Core.Controllers
{
    /// <summary>
    /// Controller that handles all api requests for the Trimming Tool app section
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TrimmingToolController : ControllerBase
    {
        private readonly ITrimmingToolService _trimmingToolService;
        // private readonly ILogger _logger;

        private ActionResult BuildAndLogErrorResult(string functionName, Exception ex)
        {
            var errorMessage = $"An error occured when processing the {functionName} method of the {nameof(TrimmingToolController)}: {ex.Message}";
            // _logger.Error(errorMessage, ex);
            return StatusCode(500, errorMessage);
        }

        public TrimmingToolController(
            ITrimmingToolService trimmingToolService
            //, ILogger logger
            )
        {
            _trimmingToolService = trimmingToolService;
            // _logger = logger;
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
        
        /// <summary>
        /// Gets a dto containing a list of set totals, and summary info of current trimmed cards
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult<TrimmingToolOverview>> GetTrimmingToolOverview()
        {
            try
            {
                var result = await _trimmingToolService.GetTrimmingToolOverview();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorResult(nameof(GetTrimmingToolOverview), ex);
            }
        }
        
        /// <summary>
        /// Gets a list of cards that can be trimmed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<TrimmingToolQueryResult>>> GetTrimmingToolCards([FromBody] TrimmingToolRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.SetCode)) return new List<TrimmingToolQueryResult>();
                var result = await _trimmingToolService.GetTrimmingToolCards(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorResult(nameof(GetTrimmingToolCards), ex);
            }
        }

        /// <summary>
        /// Trims a batch of cards
        /// Trimmed cards are just moved to the sell list
        /// </summary>
        /// <param name="cardsToTrim"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> TrimCards([FromBody] List<TrimmedCardDto> cardsToTrim)
        {
            try
            {
                await _trimmingToolService.TrimCards(cardsToTrim);
                return Ok();
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorResult(nameof(TrimCards), ex);
            }
        }
        
        /// <summary>
        /// Gets a list of current trimmed cards
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult> GetCurrentTrimmedCards()
        {
            try
            {
                var result = await _trimmingToolService.GetCurrentTrimmedCards();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorResult(nameof(GetCurrentTrimmedCards), ex);
            }
        }

        /// <summary>
        /// Removes a card from trimmed cards (back to inventory)
        /// </summary>
        /// <param name="inventoryCardId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> RestoreTrimmedCard(int inventoryCardId)
        {
            try
            {
                await _trimmingToolService.RestoreTrimmedCard(inventoryCardId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorResult(nameof(RestoreTrimmedCard), ex);
            }
        }
        
        /// <summary>
        /// Gets an export of current trimmed cards as a string
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult<string>> GetTrimmedCardsExport()
        {
            try
            {
                var result = await _trimmingToolService.GetTrimmedCardsExport();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorResult(nameof(GetTrimmedCardsExport), ex);
            }
        }
        
        /// <summary>
        /// Deletes all trimmed cards currently on the sell list
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> DeleteCurrentTrimmedCards()
        {
            try
            {
                await _trimmingToolService.DeleteCurrentTrimmedCards();
                return Ok();
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorResult(nameof(DeleteCurrentTrimmedCards), ex);
            }
        }
    }
}