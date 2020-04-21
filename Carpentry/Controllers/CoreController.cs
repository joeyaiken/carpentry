//using Carpentry.Interfaces;
using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using Carpentry.UI.Legacy.Models;
using Carpentry.UI.Legacy.Util;
//using Carpentry.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Legacy.Controllers
{
    [Route("api/[controller]")]
    public class CoreController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Core controller: {ex.Message}";
        }

        private readonly ICoreControllerService _coreService;
        private readonly MapperService _mapper;

        public CoreController(
            ICoreControllerService coreService,
            MapperService mapper
            )
        {
            _coreService = coreService;
            _mapper = mapper;
        }

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
            try
            {
                AppFiltersDto result = await _coreService.GetAppFilterValues();

                LegacyAppFiltersDto mappedResult = new LegacyAppFiltersDto
                {
                    Formats = _mapper.ToLegacy(result.Formats),
                    ManaColors = _mapper.ToLegacy(result.ManaColors),
                    Rarities = _mapper.ToLegacy(result.Rarities),
                    Sets = _mapper.ToLegacy(result.Sets),
                    Statuses = _mapper.ToLegacy(result.Statuses),
                    Types = _mapper.ToLegacy(result.Types),
                };

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetFilterValues", ex));
            }
        }
    }
}
