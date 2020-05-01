using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;
//using Carpentry.Service.Interfaces;
//using Carpentry.Service.Models;

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
        public IActionResult GetStatus()
        {
            return Ok("Online");
        }

        //Add
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddCard([FromBody] InventoryCardDto dto)
        {
            try
            {
                var updatedId = await _inventory.AddInventoryCard(dto);
                return Ok(updatedId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Add", ex));
            }
        }

        //AddCardBatch
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddCardBatch([FromBody] List<InventoryCardDto> dto)
        {
            try
            {
                await _inventory.AddInventoryCardBatch(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddBatch", ex));
            }
        }

        //Update
        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateCard([FromBody] InventoryCardDto dto)
        {
            try
            {
                await _inventory.UpdateInventoryCard(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Update", ex));
            }
        }

        //Delete
        [HttpGet("[action]")]
        public async Task<ActionResult> DeleteCard(int id)
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
        public async Task<ActionResult<IEnumerable<InventoryOverviewDto>>> SearchCards([FromBody] InventoryQueryParameter param)
        {
            try
            {
                IEnumerable<InventoryOverviewDto> result = await _inventory.GetInventoryOverviews(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Search", ex));
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<InventoryDetailDto>> GetCardsByName(string name)
        {
            try
            {
                InventoryDetailDto result = await _inventory.GetInventoryDetailByName(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetByName", ex));
            }
        }
    }
}
