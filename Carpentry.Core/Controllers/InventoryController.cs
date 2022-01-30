using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Search;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the {nameof(InventoryController)}: {ex.Message}";
        }

        private readonly IInventoryService _trimmingToolService;
        private readonly IDataExportService _dataExportService;
        private readonly IDataImportService _dataImportService;
        private readonly ISearchService _searchService;

        /// <summary>
        /// Constructor, uses DI to get a card repo
        /// </summary>
        public InventoryController(
            IInventoryService trimmingToolService,
            IDataExportService dataExportService,
            IDataImportService dataImportService,
            ISearchService searchService
            )
        {
            _trimmingToolService = trimmingToolService;
            _dataExportService = dataExportService;
            _dataImportService = dataImportService;
            _searchService = searchService;
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

        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddInventoryCard([FromBody] InventoryCardDto dto)
        {
            try
            {
                var updatedId = await _trimmingToolService.AddInventoryCard(dto);
                return Ok(updatedId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddInventoryCard", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AddInventoryCardBatch([FromBody] List<NewInventoryCard> dto)
        {
            try
            {
                await _trimmingToolService.AddInventoryCardBatch(dto);
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
                await _trimmingToolService.UpdateInventoryCard(dto);
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
                await _trimmingToolService.UpdateInventoryCardBatch(batch);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("UpdateInventoryCardBatch", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> DeleteInventoryCard(int id)
        {
            try
            {
                await _trimmingToolService.DeleteInventoryCard(id);
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
                await _trimmingToolService.DeleteInventoryCardBatch(batchIDs);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("DeleteInventoryCardBatch", ex));
            }
        }

        #endregion

        #region Search

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<InventoryOverviewDto>>> SearchCards([FromBody] InventoryQueryParameter param)
        {
            try
            {
                IEnumerable<InventoryOverviewDto> result = await _searchService.SearchInventoryCards(param);
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
                var result = await _trimmingToolService.GetInventoryDetail(cardId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetByName", ex));
            }
        }

        #endregion

        #region Import

        [HttpPost("[action]")]
        public async Task<ActionResult<ValidatedCarpentryImportDto>> ValidateCarpentryImport(CardImportDto cardImportDto)
        {
            try
            {
                var result = await _dataImportService.ValidateCarpentryImport(cardImportDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ValidateCarpentryImport", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AddValidatedCarpentryImport(ValidatedCarpentryImportDto dto)
        {
            try
            {
                await _dataImportService.AddValidatedCarpentryImport(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddValidatedCarpentryImport", ex));
            }
        }

        #endregion

        #region Export

        [HttpGet("[action]")]
        public async Task<IActionResult> ExportInventoryBackup()
        {
            try
            {
                const string contentType = "application/zip";
                var resultStream = await _dataExportService.GenerateZipBackup();
                return File(resultStream, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ExportInventoryBackup", ex));
            }
        }

        #endregion

        #region Trimming Tool

        
        #endregion
    }



}
