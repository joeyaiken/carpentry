using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;

namespace Carpentry.UI.Controllers
{
    /// <summary>
    /// This controller will return general/core app information
    /// Eventually, it could be the spot where backups & updates are requested, but that hasn't been designed yet
    /// </summary>
    [Route("api/[controller]")]
    public class CoreController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Core controller: {ex.Message}";
        }

        private readonly IFilterService _filterService;
        private readonly IDataBackupService _backupService;

        public CoreController(IFilterService filterService, IDataBackupService backupService)
        {
            _filterService = filterService;
            _backupService = backupService;
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
        public async Task<ActionResult<AppFiltersDto>> GetFilterValues()
        {
            try
            {
                AppFiltersDto result = await _filterService.GetAppFilterValues();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetFilterValues", ex));
            }
        }

        //get backup details / props (string directory)
        [HttpPost("[action]")]
        public async Task <ActionResult<BackupDetailDto>> GetBackupDetails(string directory)
        {
            try
            {
                BackupDetailDto result = await _backupService.GetBackupDetail(directory);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetBackupDetails", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> BackupInventoryData(string directory)
        {
            try
            {
                await _backupService.BackupDatabase(directory);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetBackupDetails", ex));
            }
        }



        //Possible controller methods

        //Get tracked sets
        //  
        
        //Get all possible sets (there are like 500)
        //  Paginated?
        //  Query scryfall directly for this, or cache it?
        //      Maybe it's something that's ALSO updated daily?


        //add tracked set

        //update tracked set

        //update the prices of a single set
        //  ??break this down??





    }
}
