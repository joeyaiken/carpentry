using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Service.Interfaces;

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

        private readonly ICarpentryCoreService _coreService;

        public CoreController(ICarpentryCoreService coreService)
        {
            _coreService = coreService;
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

        #region Filter Options

        /// <summary>
        /// Returns default reference/filter values used by the app
        /// When the app loads, values will be queried to populate dropdown lists
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult<AppFiltersDto>> GetFilterValues()
        {
            try
            {
                AppFiltersDto result = await _coreService.GetAppFilterValues();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetFilterValues", ex));
            }
        }

        #endregion Filter Options

        #region Tracked Sets

        [HttpGet("[action]")]
        public async Task<ActionResult<List<SetDetailDto>>> GetTrackedSets(bool showUntracked, bool update = false)
        {
            try
            {
                var result = await _coreService.GetTrackedSets(showUntracked, update);
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
                await _coreService.AddTrackedSet(setId);
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
                await _coreService.UpdateTrackedSet(setId);
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
                await _coreService.RemoveTrackedSet(setId);
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
