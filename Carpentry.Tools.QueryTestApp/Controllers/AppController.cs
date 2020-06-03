using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Tools.QueryTestApp.Controllers
{

    [Route("api/[controller]")]
    public class AppController : ControllerBase
    {
        private readonly IInventoryService _inventory;

        public AppController(IInventoryService inventory)
        {
            _inventory = inventory;
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

        /// <summary>
        /// Returns default reference/filter values used by the app
        /// When the app loads, values will be queried to populate dropdown lists
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult<InventoryOverviewDto>> CallTestQuery([FromBody] InventoryQueryParameter param)
        {
            try
            {
                IEnumerable<InventoryOverviewDto> result = await _inventory.GetInventoryOverviews(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured calling GetInventoryOverviews: {ex.Message}");
            }
        }
    }
}
