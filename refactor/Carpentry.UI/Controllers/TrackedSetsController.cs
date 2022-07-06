using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.Logic;
using Carpentry.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Controllers;

public class TrackedSetsController : ControllerBase
{
    private static string FormatExceptionMessage(string functionName, Exception ex)
    {
        return $"An error occured when processing the {functionName} method of the {nameof(TrackedSetsController)}: {ex.Message}";
    }
    
    private readonly IDataUpdateService _dataUpdateService;
    
    public TrackedSetsController(IDataUpdateService dataUpdateService)
    {
        _dataUpdateService = dataUpdateService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<NormalizedList<TrackedSetDto>>>> GetTrackedSets(bool showUntracked)
    {
        try
        {
            var serviceResult = await _dataUpdateService.GetTrackedSets(showUntracked);
            var mappedResult = serviceResult.Select(s => new TrackedSetDto()
            {
                SetId = s.SetId,
                Code = s.Code,
                Name = s.Name,
                
                // CollectedCount = s.CollectedCount,
            }).ToList();
            var result = new NormalizedList<TrackedSetDto>(mappedResult, s => s.SetId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, FormatExceptionMessage("GetTrackedSets", ex));
        }
    }

}