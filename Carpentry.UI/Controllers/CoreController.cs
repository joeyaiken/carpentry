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
        //TODO - consider breaking this into multiple controllers
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Core controller: {ex.Message}";
        }

        private readonly IFilterService _filterService;
        private readonly IDataBackupService _backupService;
        private readonly ICardImportService _importService;
        private readonly IDataUpdateService _updateService;

        public CoreController(
            IFilterService filterService,
            IDataBackupService backupService,
            ICardImportService importService,
            IDataUpdateService updateService
        )
        {
            _filterService = filterService;
            _backupService = backupService;
            _importService = importService;
            _updateService = updateService;
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

        #region filters

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

        #endregion

        #region tracked set definitions

        //-- all about managing card definitions - "card data" controller?
        //
        //GetTrackedSets(can this be the same source the DDLs pull from?)
        [HttpGet("[action]")]
        public async Task<ActionResult<List<SetDetailDto>>> GetTrackedSets(
                bool showUntracked,
                bool update = false
            )
        {
            try
            {
                var result = await _updateService.GetTrackedSets(showUntracked, update);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetTrackedSets", ex));
            }
        }

        //Get Tracked Set Detail ?

        //UpdateTrackedSetScryData
        [HttpGet("[action]")]
        public async Task<ActionResult> UpdateTrackedSetScryData(string setCode)
        {
            try
            {
                await _updateService.UpdateTrackedSetScryData(setCode);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("UpdateTrackedSetScryData", ex));
            }
        }
        //UpdateTrackedSetCardData
        [HttpGet("[action]")]
        public async Task<ActionResult> UpdateTrackedSetCardData(string setCode)
        {
            try
            {
                await _updateService.UpdateTrackedSetCardData(setCode);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("UpdateTrackedSetCardData", ex));
            }
        }
        //GetAllAvailableSets
        //[HttpGet("[action]")]
        //public async Task<ActionResult<List<SetDetailDto>>> GetAllAvailableSets()
        //{
        //    try
        //    {
        //        //var result = await _updateService.GetAllAvailableSets();
        //        //return Ok(result);
        //        throw new NotImplementedException();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("GetAllAvailableSets", ex));
        //    }
        //}

        //GetUntrackedSets ??



        //AddTrackedSet
        //[HttpGet("[action]")]
        //public async Task<ActionResult> AddTrackedSet(string setCode)
        //{
        //    try
        //    {
        //        await _updateService.AddTrackedSet(setCode);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("AddTrackedSet", ex));
        //    }
        //}
        ////RemoveTrackedSet(fails if any inventory cards present)
        //[HttpGet("[action]")]
        //public async Task<ActionResult> RemoveTrackedSet(string setCode)
        //{
        //    try
        //    {
        //        await _updateService.RemoveTrackedSet(setCode);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("RemoveTrackedSet", ex));
        //    }
        //}

        #endregion

        #region backups

        //-- all about managing collection (and kinda inventory) data
        //VerifyBackupLocation
        [HttpGet("[action]")]
        public async Task<ActionResult<BackupDetailDto>> VerifyBackupLocation(string directory)
        {
            try
            {
                var result = await _backupService.VerifyBackupLocation(directory);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("VerifyBackupLocation", ex));
            }
        }
        
        //BackupCollection
        [HttpGet("[action]")]
        public async Task<ActionResult> BackupCollection(string directory)
        {
            try
            {
                //read from config??
                //The backup service really shouldn't be concerned with this nonsense


                await _backupService.BackupCollection(directory);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("BackupCollection", ex));
            }
        }
        
        //RestoreCollectionFromBackup
        [HttpGet("[action]")]
        public async Task<ActionResult> RestoreCollectionFromBackup(string directory)
        {
            try
            {
                await _backupService.RestoreCollectionFromBackup(directory);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("RestoreCollectionFromBackup", ex));
            }
        }

        #endregion

        #region import

        //ValidateImport
        [HttpGet("[action]")]
        public async Task<ActionResult<ValidatedCardImportDto>> ValidateImport(CardImportDto payload)
        {
            try
            {
                var result = await _importService.ValidateImport(payload);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("ValidateImport", ex));
            }
        }
        //AddValidatedImport
        [HttpGet("[action]")]
        public async Task<ActionResult> AddValidatedImport(ValidatedCardImportDto validatedPayload)
        {
            try
            {
                await _importService.AddValidatedImport(validatedPayload);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddValidatedImport", ex));
            }
        }

        #endregion

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
