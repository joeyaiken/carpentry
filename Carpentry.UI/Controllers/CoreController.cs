//using Carpentry.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.UI.Models;
//using Carpentry.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Carpentry.UI.Controllers
{
    [Route("api/[controller]")]
    public class CoreController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Core controller: {ex.Message}";
        }

        private readonly ICarpentryService _carpentry;

        public CoreController(ICarpentryService carpentry)
        {
            _carpentry = carpentry;
        }

        ///// <summary>
        ///// This method just ensures the controller can start correctly (catches DI issues)
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("Online");
        //}

        //[HttpGet("[action]")]
        //public async Task<ActionResult<FilterOptionDto>> GetFilterValues()
        //{
        //    try
        //    {
        //        var filters = await _carpentry.GetAppFilterValues();
        //        return Ok(filters);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("GetFilterValues", ex));
        //    }
        //}

        ////Backup DB
        ////should this be a POST since it could/should include filepath info?
        //[HttpGet("[action]")]
        //public async Task<ActionResult> BackupDatabase()
        //{
        //    throw new NotImplementedException();
        //}

        ////Restore DB
        //[HttpGet("[action]")]
        //public async Task<ActionResult> RestoreDatabase()
        //{
        //    throw new NotImplementedException();
        //}

        ////Get Set|Data Update Status
        //[HttpGet("[action]")]
        //public async Task<ActionResult> GetDatabaseUpdateStatus()
        //{
        //    throw new NotImplementedException();
        //}


        ////Update Set Scry Data
        //[HttpGet("[action]")]
        //public async Task<ActionResult> UpdateScryfallSet(string setCode)
        //{
        //    throw new NotImplementedException();
        //}

        ////Update Set Card Data
        //[HttpGet("[action]")]
        //public async Task<ActionResult> UpdateSetData(string setCode)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
