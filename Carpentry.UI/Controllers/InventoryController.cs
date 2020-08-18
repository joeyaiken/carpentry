using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using Carpentry.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Inventory controller: {ex.Message}";
        }

        private readonly ICarpentryInventoryService _inventory;

        /// <summary>
        /// Constructor, uses DI to get a card repo
        /// </summary>
        /// <param name="repo"></param>
        public InventoryController(ICarpentryInventoryService inventory)
        {
            _inventory = inventory;
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

        #region Inventory Cards

        //Add
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddInventoryCard([FromBody] InventoryCardDto dto)
        {
            try
            {
                var updatedId = await _inventory.AddInventoryCard(dto);
                return Ok(updatedId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddInventoryCard", ex));
            }
        }

        //AddCardBatch
        [HttpPost("[action]")]
        public async Task<ActionResult> AddInventoryCardBatch([FromBody] List<InventoryCardDto> dto)
        {
            try
            {
                await _inventory.AddInventoryCardBatch(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddInventoryCardBatch", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateInventoryCard([FromBody] InventoryCardDto dto)
        {
            try
            {
                await _inventory.UpdateInventoryCard(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("UpdateInventoryCard", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateInventoryCardBatch([FromBody] List<InventoryCardDto> batch)
        {
            try
            {
                await _inventory.UpdateInventoryCardBatch(batch);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("UpdateInventoryCardBatch", ex));
            }
        }

        //Delete
        [HttpGet("[action]")]
        public async Task<ActionResult> DeleteInventoryCard(int id)
        {
            try
            {
                await _inventory.DeleteInventoryCard(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("DeleteInventoryCard", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> DeleteInventoryCardBatch(List<int> batchIDs)
        {
            try
            {
                await _inventory.DeleteInventoryCardBatch(batchIDs);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("DeleteInventoryCardBatch", ex));
            }
        }

        #endregion

        #region Search

        //Search
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<InventoryOverviewDto>>> SearchCards([FromBody] InventoryQueryParameter param)
        {
            try
            {
                IEnumerable<InventoryOverviewDto> result = await _inventory.GetInventoryOverviews(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Search", ex));
            }
        }

        /// <summary>
        /// Loads an inventory detail for a given card ID
        /// Returns data the given card, and all other cards with the same Name
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult<InventoryDetailDto>> GetInventoryDetail(int cardId)
        {
            try
            {
                InventoryDetailDto result = await _inventory.GetInventoryDetail(cardId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetByName", ex));
            }
        }

        #endregion

        #region Collection Builder
        
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<InventoryOverviewDto>>> GetCollectionBuilderSuggestions()
        {
            try
            {
                List<InventoryOverviewDto> result = await _inventory.GetCollectionBuilderSuggestions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetCollectionBuilderSuggestions", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> HideCollectionBuilderSuggestion(InventoryOverviewDto dto)
        {
            try
            {
                await _inventory.HideCollectionBuilderSuggestion(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("HideCollectionBuilderSuggestion", ex));
            }
        }

        #endregion

        #region Trimming Tips

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<InventoryOverviewDto>>> GetTrimmingTips()
        {
            try
            {
                List<InventoryOverviewDto> result = await _inventory.GetTrimmingTips();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetTrimmingTips", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> HideTrimmingTip(InventoryOverviewDto dto)
        {
            try
            {
                await _inventory.HideTrimmingTip(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("HideTrimmingTip", ex));
            }
        }

        #endregion

        #region Import

        //Validate Carpentry Import
        [HttpPost("[action]")]
        public async Task<ActionResult<ValidatedCarpentryImportDto>> ValidateCarpentryImport(CardImportDto cardImportDto)
        {
            try
            {
                var result = await _inventory.ValidateCarpentryImport(cardImportDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ValidateCarpentryImport", ex));
            }
        }

        //Add Validated Carpentry Import
        [HttpPost("[action]")]
        public async Task<ActionResult> AddValidatedCarpentryImport(ValidatedCarpentryImportDto dto)
        {
            try
            {
                await _inventory.AddValidatedCarpentryImport(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddValidatedCarpentryImport", ex));
            }
        }

        #endregion

        #region Export

        //ExportInventoryBackup
        [HttpGet("[action]")]
        public async Task<IActionResult> ExportInventoryBackup()
        {
            try
            {
                const string contentType = "application/zip";
                var resultStream = await _inventory.ExportInventoryBackup();
                //var resultStream = await _dataBackupService.GenerateZipBackup();
                return File(resultStream, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ExportInventoryBackup", ex));
            }
        }

        #endregion
    }
}
