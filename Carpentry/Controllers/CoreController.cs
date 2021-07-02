using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.CarpentryData.Models.QueryResults;

namespace Carpentry.Controllers
{
    /// <summary>
    /// This controller will return general/core app information.
    /// Currently, filter options, and managing tracked set data.
    /// </summary>
    [Route("api/[controller]")]
    public class CoreController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Core controller: {ex.Message}";
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

        #region Other

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
                //ummm does this call a single 'core service' or does it
                await _dataUpdateService.ValidateDatabase();
                AppFiltersDto result = await _filterService.GetAppCoreData();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetCoreData", ex));
            }
        }

        /// <summary>
        /// Returns default reference/filter values used by the app
        /// When the app loads, values will be queried to populate dropdown lists
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult> ValidateDatabase()
        {
            try
            {
                //ummm does this call a single 'core service' or does it
                await _dataUpdateService.ValidateDatabase();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ValidateDatabase", ex));
            }
        }


        //_inventoryService
        //public async Task<IEnumerable<InventoryTotalsByStatusResult>> GetCollectionTotals()
        [HttpGet("[action]")]
        public async Task<ActionResult<InventoryTotalsByStatusResult>> GetCollectionTotals()
        {
            try
            {
                //ummm does this call a single 'core service' or does it
                var result = await _inventoryService.GetCollectionTotals();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetCollectionTotals", ex));
            }
        }

        #endregion 

        #region Tracked Sets

        [HttpGet("[action]")]
        public async Task<ActionResult<List<SetDetailDto>>> GetTrackedSets(bool showUntracked, bool update = false)
        {
            try
            {
                var result = await _dataUpdateService.GetTrackedSets(showUntracked, update);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetTrackedSets", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> AddTrackedSet(int setId)
        {
            try
            {
                await _dataUpdateService.AddTrackedSet(setId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddTrackedSet", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> UpdateTrackedSet(int setId)
        {
            try
            {
                await _dataUpdateService.UpdateTrackedSet(setId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("UpdateTrackedSet", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> RemoveTrackedSet(int setId)
        {
            try
            {
                await _dataUpdateService.RemoveTrackedSet(setId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("RemoveTrackedSet", ex));
            }
        }

        #endregion Tracked Sets



    }
}
