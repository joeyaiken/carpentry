using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.Logic;
using Carpentry.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Controllers;

/// <summary>
/// This controller delivers information on Tracked Sets
/// (Carpentry stores a record of all existing sets, but only stores card records for tracked sets)
/// </summary>
[Route("api/[controller]")]
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
    
    /// <summary>
    /// This method just ensures the controller can start correctly (catches DI issues)
    /// </summary>
    /// <returns></returns>
    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok("Online");
    }
    
    [HttpGet]
    public async Task<ActionResult<NormalizedList<TrackedSetDto>>> GetTrackedSets(bool showUntracked)
    {
        try
        {
            var serviceResult = await _dataUpdateService.GetTrackedSets(showUntracked);
            var mappedResult = serviceResult.Select(TrackedSetDto.FromModel).ToList();
            var result = new NormalizedList<TrackedSetDto>(mappedResult, s => s.SetId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, FormatExceptionMessage("GetTrackedSets", ex));
        }
    }

}