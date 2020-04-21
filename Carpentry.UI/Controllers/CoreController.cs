﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;

namespace Carpentry.UI.Controllers
{
    [Route("api/[controller]")]
    public class CoreController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Core controller: {ex.Message}";
        }

        private readonly ICoreControllerService _coreService;

        public CoreController(ICoreControllerService coreService)
        {
            _coreService = coreService;
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
                AppFiltersDto result = await _coreService.GetAppFilterValues();
                return Ok(result);

                ////List<FilterOption> formats = await _filterService.GetFormatFilterOptions();

                ////List<FilterOptionDto> mappedFormats = formats.Select(x => new FilterOptionDto(x)).ToList();

                ////List<FilterOptionDto> anotherWay = (await _filterService.GetFormatFilterOptions()).Select(x => new FilterOptionDto(x)).ToList();

                //AppFiltersDto filters = n ew AppFiltersDto
                //{
                //    Formats = _mapper.ToDto(await _filterService.GetFormatFilterOptions()),
                //    ManaColors = _mapper.ToDto(await _filterService.GetManaColorFilterOptions()),
                //    Rarities = _mapper.ToDto(await _filterService.GetRarityFilterOptions()),
                //    Sets = _mapper.ToDto(await _filterService.GetSetFilterOptions()),
                //    Statuses = _mapper.ToDto(await _filterService.GetCardStatusFilterOptions()),
                //    Types = _mapper.ToDto(await _filterService.GetTypeFilterOptions()),
                //};

                //return Ok(filters);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetFilterValues", ex));
            }
        }

        ////Backup DB
        ////should this be a POST since it could/should include filepath info?
        //[HttpGet("[action]")]
        //public async Task<ActionResult> BackupDatabase()
        //{
        //    await Task.Delay(0);
        //    throw new NotImplementedException();
        //}

        ////Restore DB
        //[HttpGet("[action]")]
        //public async Task<ActionResult> RestoreDatabase()
        //{
        //    await Task.Delay(0);
        //    throw new NotImplementedException();
        //}

        ////Get Set|Data Update Status
        //[HttpGet("[action]")]
        //public async Task<ActionResult> GetDatabaseUpdateStatus()
        //{
        //    await Task.Delay(0);
        //    throw new NotImplementedException();
        //}


        ////Update Set Scry Data
        //[HttpGet("[action]")]
        //public async Task<ActionResult> UpdateScryfallSet(string setCode)
        //{
        //    await Task.Delay(0);
        //    throw new NotImplementedException();
        //}

        ////Update Set Card Data
        //[HttpGet("[action]")]
        //public async Task<ActionResult> UpdateSetData(string setCode)
        //{
        //    await Task.Delay(0);
        //    throw new NotImplementedException();
        //}
    }
}
