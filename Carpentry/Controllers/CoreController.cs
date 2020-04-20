//using Carpentry.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
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

        private readonly IFilterService _filterService;
        private readonly IMapperService _mapper;

        public CoreController(
            IFilterService filterService,
            IMapperService mapper
            )
        {
            _filterService = filterService;
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
                //List<FilterOption> formats = await _filterService.GetFormatFilterOptions();

                //List<FilterOptionDto> mappedFormats = formats.Select(x => new FilterOptionDto(x)).ToList();

                //List<FilterOptionDto> anotherWay = (await _filterService.GetFormatFilterOptions()).Select(x => new FilterOptionDto(x)).ToList();

                LegacyAppFiltersDto filters = new LegacyAppFiltersDto
                {
                    Formats = _mapper.ToDto(await _filterService.GetFormatFilterOptions()),
                    ManaColors = _mapper.ToDto(await _filterService.GetManaColorFilterOptions()),
                    Rarities = _mapper.ToDto(await _filterService.GetRarityFilterOptions()),
                    Sets = _mapper.ToDto(await _filterService.GetSetFilterOptions()),
                    Statuses = _mapper.ToDto(await _filterService.GetCardStatusFilterOptions()),
                    Types = _mapper.ToDto(await _filterService.GetTypeFilterOptions()),
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
