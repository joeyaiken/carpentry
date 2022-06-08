using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Logic;
using Carpentry.Logic.Models;
using Carpentry.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.Controllers
{
    /// <summary>
    /// This controller delivers information on Tracked Sets
    /// (Carpentry stores a record of all existing sets, but only stores card records for tracked sets)
    /// </summary>
    [Route("api/[controller]")]
    public class TrackedSetsController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the {nameof(TrackedSetsController)}: {ex.Message}";
        }

        private readonly IDataUpdateService _dataUpdateService;
        
        public TrackedSetsController(IDataUpdateService dataUpdateService)
        {
            _dataUpdateService = dataUpdateService;
        }
        
        /// <summary>
        /// This method just ensures the controller can start correctly (catches DI issues)
        /// </summary>
        /// <returns></returns>
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("Online");
        }
        
        [HttpGet]
        public async Task<ActionResult<List<NormalizedList<SetDetailDto>>>> GetTrackedSets(bool showUntracked, bool update = false)
        {
            try
            {
                var result = await _dataUpdateService.GetTrackedSets(showUntracked, update);
                var mappedResults = new NormalizedList<SetDetailDto>(result, s => s.SetId);
                return Ok(mappedResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetTrackedSets", ex));
            }
        }
        
        [HttpPost("add")]
        public async Task<ActionResult> AddTrackedSet([FromBody] int setId)
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

        [HttpPost("update")]
        public async Task<ActionResult> UpdateTrackedSet([FromBody] int setId)
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

        [HttpPost("remove")]
        public async Task<ActionResult> RemoveTrackedSet([FromBody] int setId)
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
    }
}