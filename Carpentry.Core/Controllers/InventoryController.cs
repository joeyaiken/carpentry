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
            return $"An error occured when processing the {functionName} method of the Inventory controller: {ex.Message}";
        }

        private readonly IInventoryService _inventoryService;
        private readonly IDataExportService _dataExportService;
        private readonly IDataImportService _dataImportService;
        private readonly ISearchService _searchService;

        /// <summary>
        /// Constructor, uses DI to get a card repo
        /// </summary>
        /// <param name="repo"></param>
        public InventoryController(
            IInventoryService inventoryService,
            IDataExportService dataExportService,
            IDataImportService dataImportService,
            ISearchService searchService
            )
        {
            _inventoryService = inventoryService;
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
                var updatedId = await _inventoryService.AddInventoryCard(dto);
                return Ok(updatedId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddInventoryCard", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AddInventoryCardBatch([FromBody] List<InventoryCardDto> dto)
        {
            try
            {
                await _inventoryService.AddInventoryCardBatch(dto);
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
                await _inventoryService.UpdateInventoryCard(dto);
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
                await _inventoryService.UpdateInventoryCardBatch(batch);
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
                await _inventoryService.DeleteInventoryCard(id);
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
                await _inventoryService.DeleteInventoryCardBatch(batchIDs);
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
                InventoryDetailDto result = await _inventoryService.GetInventoryDetail(cardId);
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

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<InventoryOverviewDto>>> GetTrimmingToolCards([FromBody] TrimmingToolRequest request) //payload: TrimmingTool[Card]Request
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.SetCode)) return new List<InventoryOverviewDto>();

                //var result = new List<InventoryOverviewDto>();

                var result = await _inventoryService.GetTrimmingToolCards(request.SetCode, request.MinCount, request.FilterBy, request.SearchGroup);
                //InventoryQueryParameter param = new InventoryQueryParameter()
                //{
                //    Colors = new List<string>(),
                //    ExclusiveColorFilters = false,
                //    GroupBy = "print",
                //    MaxCount = 0,
                //    MinCount = request.MinCount,
                //    MultiColorOnly = false,
                //    Rarity = new List<string>(),
                //    Set = request.SetCode,
                //    Skip = 0,
                //    Take = 25,
                //    Sort = "name",
                //    SortDescending = false,
                //    Text = "",
                //    Type = "",
                //};

                //IEnumerable<InventoryOverviewDto> result = await _searchService.SearchInventoryCards(param);
                //Task<List<CardSearchResultDto>> SearchCardDefinitions(CardSearchQueryParameter filters);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetTrimmingToolCards", ex));
            }
        }

        //save payload of cards
        [HttpPost("[action]")]
        public async Task<ActionResult> TrimCards([FromBody] List<TrimmedCardDto> cardsToTrim)
        {
            try
            {
                await _inventoryService.TrimCards(cardsToTrim);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("TrimCards", ex));
            }

        }

        #endregion
    }


    
}
