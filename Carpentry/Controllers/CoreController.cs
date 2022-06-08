using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic;
using Carpentry.Logic.Models;
using Carpentry.CarpentryData.Models;
using Carpentry.Controllers;
using Carpentry.Models;

namespace Carpentry.Controllers
{
    /// <summary>
    /// This controller will return general/core app information.
    /// Currently, filter options, and managing tracked set data.
    /// </summary>
    [Route("api/[controller]")]
    public class CoreController : ControllerBase
    {
        private static string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the {nameof(CoreController)}: {ex.Message}";
        }

        private readonly IDataUpdateService _dataUpdateService;
        private readonly IFilterService _filterService;
        private readonly IInventoryService _inventoryService;

        public CoreController(IDataUpdateService dataUpdateService, IFilterService filterService, IInventoryService inventoryService)
        {
            _dataUpdateService = dataUpdateService;
            _filterService = filterService;
            _inventoryService = inventoryService;
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
        /// Returns default reference/filter values used by the app
        /// When the app loads, values will be queried to populate dropdown lists
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult<AppFiltersDto>> GetCoreData()
        {
            try
            {
                await _dataUpdateService.ValidateDatabase();
                AppFiltersDto result = await _filterService.GetAppCoreData();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetCoreData", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<NormalizedList<InventoryTotalsByStatusResult>>> GetCollectionTotals()
        {
            try
            {
                var result = await _inventoryService.GetCollectionTotals();
                var mappedResult = new NormalizedList<InventoryTotalsByStatusResult>(result, r => r.StatusId);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetCollectionTotals", ex));
            }
        }
    }
}
