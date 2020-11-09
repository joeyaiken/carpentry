using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.Legacy.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.Legacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        /// <summary>
        /// Constructor, uses DI to get a card repo
        /// </summary>
        /// <param name="repo"></param>
        public InventoryController() { }

        /// <summary>
        /// This method just ensures the controller can start correctly (catches DI issues)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Online");
        }

        //Add
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> Add([FromBody] LegacyInventoryCardDto dto)
        {
            var result = 1;
            return await Task.FromResult(result);
        }

        //AddCardBatch
        [HttpPost("[action]")]
        public async Task<ActionResult> AddBatch([FromBody] List<LegacyInventoryCardDto> dto)
        {
            return await Task.FromResult(Ok());
        }

        //Update
        [HttpPost("[action]")]
        public async Task<ActionResult> Update([FromBody] LegacyInventoryCardDto dto)
        {
            return await Task.FromResult(Ok());
        }

        //Delete
        [HttpGet("[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Task.FromResult(Ok());
        }

        //Search
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<LegacyInventoryOverviewDto>>> Search([FromBody] InventoryQueryParameter param)
        {
            var result = new List<LegacyInventoryOverviewDto>();
            return await Task.FromResult(Ok(result));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<LegacyInventoryDetailDto>> GetByName(string name)
        {
            var result = new LegacyInventoryDetailDto();
            return await Task.FromResult(Ok(result));
        }
    }
}
