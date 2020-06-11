using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.UI.Legacy.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Legacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardSearchController : ControllerBase
    {
        public CardSearchController() { }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Online");
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<LegacyMagicCardDto>>> SearchWeb([FromBody] NameSearchQueryParameter param)
        {
            var result = new List<LegacyMagicCardDto>();
            return await Task.FromResult(Ok(result));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<LegacyMagicCardDto>>> SearchSet([FromBody] CardSearchQueryParameter filters)
        {
            var result = new List<LegacyMagicCardDto>();
            return await Task.FromResult(Ok(result));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<LegacyMagicCardDto>>> SearchInventory([FromBody] InventoryQueryParameter filters)
        {
            var result = new List<LegacyMagicCardDto>();
            return await Task.FromResult(Ok(result));
        }
    }
}
