using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.UI.Legacy.Models;
using Carpentry.UI.Legacy.Util;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Legacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Inventory controller: {ex.Message}";
        }

        private readonly IInventoryService _inventory;
        private readonly MapperService _mapper;

        /// <summary>
        /// Constructor, uses DI to get a card repo
        /// </summary>
        /// <param name="repo"></param>
        public InventoryController(IInventoryService inventory, MapperService mapper)
        {
            _inventory = inventory;
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

        //Add
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> Add([FromBody] LegacyInventoryCardDto dto)
        {
            try
            {
                var updatedId = await _inventory.AddInventoryCard(_mapper.ToModel(dto));
                return Ok(updatedId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Add", ex));
            }
        }

        //AddCardBatch
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddBatch([FromBody] List<LegacyInventoryCardDto> dto)
        {
            try
            {
                await _inventory.AddInventoryCardBatch(_mapper.ToModel(dto));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddBatch", ex));
            }
        }

        //Update
        [HttpPost("[action]")]
        public async Task<ActionResult> Update([FromBody] LegacyInventoryCardDto dto)
        {
            try
            {
                await _inventory.UpdateInventoryCard(_mapper.ToModel(dto));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Update", ex));
            }
        }

        //Delete
        [HttpGet("[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _inventory.DeleteInventoryCard(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Delete", ex));
            }
        }

        //Search
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<LegacyInventoryOverviewDto>>> Search([FromBody] InventoryQueryParameter param)
        {
            try
            {
                var result = await _inventory.GetInventoryOverviews(param);
                List<LegacyInventoryOverviewDto> mappedResult = _mapper.ToLegacy(result);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Search", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<LegacyInventoryDetailDto>> GetByName(string name)
        {
            try
            {
                var result = await _inventory.GetInventoryDetailByName(name);
                //InventoryDetailDto mappedResult = new InventoryDetailDto(result);
                LegacyInventoryDetailDto mappedResult = _mapper.ToLegacy(result);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetByName", ex));
            }
        }
    }
}
