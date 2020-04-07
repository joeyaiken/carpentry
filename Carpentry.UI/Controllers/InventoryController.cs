using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
            //try
            //{
            //    var updatedId = await _carpentry.AddInventoryCard(dto);
            //    return Accepted(updatedId);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, FormatExceptionMessage("Add", ex));
            //}


        }

        //AddCardBatch
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddBatch([FromBody] IEnumerable<InventoryCardDto> dto)
        {
            throw new NotImplementedException();
            //try
            //{
            //    await _carpentry.AddInventoryCardBatch(dto);
            //    return Accepted();
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, FormatExceptionMessage("AddBatch", ex));
            //}


        }

        //Update
        [HttpPost("[action]")]
        public async Task<ActionResult> Update([FromBody] InventoryCardDto dto)
        {

            throw new NotImplementedException();
            //try
            //{
            //    await _carpentry.UpdateInventoryCard(dto);
            //    return Accepted();
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, FormatExceptionMessage("Update", ex));
            //}
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
            throw new NotImplementedException();
            //try
            //{
            //    var result = await _carpentry.GetInventoryOverviews(param);
            //    return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, FormatExceptionMessage("Search", ex));
            //}
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<InventoryDetailDto>> GetByName(string name)
        {
            throw new NotImplementedException();
            //try
            //{
            //    var result = await _carpentry.GetInventoryDetailByName(name);
            //    return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, FormatExceptionMessage("GetByName", ex));
            //}
        }


    }
}
