using Carpentry.Legacy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carpentry.Legacy.Controllers
{
    [Route("api/[controller]")]
    public class CoreController : ControllerBase
    {
        public CoreController() { }

        /// <summary>
        /// This method just ensures the controller can start correctly (catches DI issues)
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Online");
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<LegacyAppFiltersDto>> GetFilterValues()
        {
            var result = new LegacyAppFiltersDto();
            return await Task.FromResult(Ok(result));
        }
    }
}
