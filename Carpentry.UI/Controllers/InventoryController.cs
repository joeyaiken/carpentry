using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
//using Carpentry.Data.Models;
//using Carpentry.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Controllers
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

        /// <summary>
        /// Constructor, uses DI to get a card repo
        /// </summary>
        /// <param name="repo"></param>
        public InventoryController(IInventoryService inventory)
        {
            _inventory = inventory;
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
        public async Task<ActionResult<int>> Add([FromBody] InventoryCardDto dto)
        {
            try
            {
                var updatedId = await _inventory.AddInventoryCard(dto.ToModel());
                return Ok(updatedId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Add", ex));
            }
        }

        //AddCardBatch
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddBatch([FromBody] IEnumerable<InventoryCardDto> dto)
        {
            try
            {
                await _inventory.AddInventoryCardBatch(dto.Select(x => x.ToModel()));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddBatch", ex));
            }


        }

        //Update
        [HttpPost("[action]")]
        public async Task<ActionResult> Update([FromBody] InventoryCardDto dto)
        {
            try
            {
                await _inventory.UpdateInventoryCard(dto.ToModel());
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
        public async Task<ActionResult<IEnumerable<InventoryOverviewDto>>> Search([FromBody] InventoryQueryParameter param)
        {
            try
            {
                var result = await _inventory.GetInventoryOverviews(param);
                List<InventoryOverviewDto> mappedResult = result.Select(x => new InventoryOverviewDto(x)).ToList();
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Search", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<InventoryDetailDto>> GetByName(string name)
        {
            try
            {
                var result = await _inventory.GetInventoryDetailByName(name);
                InventoryDetailDto mappedResult = new InventoryDetailDto(result);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetByName", ex));
            }
        }


    }
}
