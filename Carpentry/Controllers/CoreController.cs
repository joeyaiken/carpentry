using Carpentry.Interfaces;
using Carpentry.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Carpentry.Controllers
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

        [HttpPost("[action]")]
        public async Task<ActionResult<FilterOptionDto>> GetFilterValues()
        {
            try
            {
                var filters = await _carpentry.GetAppFilterValues();
                return Ok(filters);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetFilterValues", ex));
            }
        }
    }
}
