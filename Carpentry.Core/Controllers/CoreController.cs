using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic;
using Carpentry.Logic.Models;
using Carpentry.CarpentryData.Models;

namespace Carpentry.Core.Controllers
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
        private readonly IInventoryService _trimmingToolService;

        public CoreController(IDataUpdateService dataUpdateService, IFilterService filterService, IInventoryService trimmingToolService)
        {
            _dataUpdateService = dataUpdateService;
            _filterService = filterService;
            _trimmingToolService = trimmingToolService;
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

        #region Filter Values (should this be a unique controller?)

        [HttpGet("[action]")]
        public async Task<ActionResult> GetCardSetFilters()
        {
            try
            {
                var result = await _filterService.GetCardSetFilters();


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetCardSetFilters", ex));
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetCardTypeFilters()
        {
            try
            {
                var result = _filterService.GetCardTypeFilters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetCardTypeFilters", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetFormatFilters()
        {
            try
            {
                var result = await _filterService.GetFormatFilters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetFormatFilters", ex));
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetManaTypeFilters()
        {
            try
            {
                var result = _filterService.GetManaTypeFilters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetManaTypeFilters", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetRarityFilters()
        {
            try
            {
                var result = await _filterService.GetRarityFilters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetRarityFilters", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetStatusFilters()
        {
            try
            {
                var result = await _filterService.GetStatusFilters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetStatusFilters", ex));
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetCardGroupFilters()
        {
            try
            {
                var result = _filterService.GetCardGroupFilters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetCardGroupFilters", ex));
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetInventorySortOptions()
        {
            try
            {
                var result = _filterService.GetInventorySortOptions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetInventorySortOptions", ex));
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetInventoryGroupOptions()
        {
            try
            {
                var result = _filterService.GetInventoryGroupOptions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetInventoryGroupOptions", ex));
            }
        }

        #endregion

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

        [HttpGet("[action]")]
        public async Task<ActionResult<InventoryTotalsByStatusResult>> GetCollectionTotals()
        {
            try
            {
                var result = await _trimmingToolService.GetCollectionTotals();
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
