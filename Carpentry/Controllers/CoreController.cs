using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.UI.Legacy.Models;
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

        private readonly IFilterService _filterService;

        public CoreController(
            IFilterService filterService
            )
        {
            _filterService = filterService;
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
        public async Task<ActionResult<AppFiltersDto>> GetFilterValues()
        {
            try
            {
                List<FilterOption> formats = await _filterService.GetFormatFilterOptions();

                List<FilterOptionDto> mappedFormats = formats.Select(x => new FilterOptionDto(x)).ToList();

                List<FilterOptionDto> anotherWay = (await _filterService.GetFormatFilterOptions()).Select(x => new FilterOptionDto(x)).ToList();

                AppFiltersDto filters = new AppFiltersDto
                {
                    Formats = 
                        (await _filterService.GetFormatFilterOptions())
                        .Select(x => new FilterOptionDto(x)).ToList(),
                    ManaColors = 
                        (await _filterService.GetManaColorFilterOptions())
                        .Select(x => new FilterOptionDto(x)).ToList(),
                    Rarities =
                        (await _filterService.GetRarityFilterOptions())
                        .Select(x => new FilterOptionDto(x)).ToList(),
                    Sets =
                        (await _filterService.GetSetFilterOptions())
                        .Select(x => new FilterOptionDto(x)).ToList(),
                    Statuses =
                        (await _filterService.GetCardStatusFilterOptions())
                        .Select(x => new FilterOptionDto(x)).ToList(),
                    Types =
                        (await _filterService.GetTypeFilterOptions())
                        .Select(x => new FilterOptionDto(x)).ToList(),
                };

                return Ok(filters);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetFilterValues", ex));
            }
        }
    }
}
