using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.Models;
using Carpentry.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Inventory controller: {ex.Message}";
        }

        //private readonly ICardRepo _cardRepo;
        private readonly ICarpentryService _carpentry;

        /// <summary>
        /// Constructor, uses DI to get a card repo
        /// </summary>
        /// <param name="repo"></param>
        public InventoryController(ICarpentryService carpentry)
        {
            _carpentry = carpentry;
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
                var updatedId = await _carpentry.AddInventoryCard(dto);
                return Accepted(updatedId);
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
                await _carpentry.AddInventoryCardBatch(dto);
                return Accepted();
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
                await _carpentry.UpdateInventoryCard(dto);
                return Accepted();
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
                await _carpentry.DeleteInventoryCard(id);
                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("Delete", ex));
            }
        }

        //Search
        [HttpPost("[action]")]
        public async Task<ActionResult> Search([FromBody] InventoryQueryParameter param)
        {
            try
            {
                var result = await _carpentry.GetInventoryOverviews(param);
                return Ok(result);
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
                var result = await _carpentry.GetInventoryDetailByName(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetByName", ex));
            }
        }


    }
}
