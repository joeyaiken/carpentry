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

        #region Inventory Cards

        //Add
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddInventoryCard([FromBody] InventoryCardDto dto)
        {
            try
            {
                var updatedId = await _inventory.AddInventoryCard(dto);
                return Ok(updatedId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddInventoryCard", ex));
            }
        }

        //AddCardBatch
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> AddInventoryCardBatch([FromBody] List<InventoryCardDto> dto)
        {
            try
            {
                await _inventory.AddInventoryCardBatch(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("AddInventoryCardBatch", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateInventoryCard([FromBody] InventoryCardDto dto)
        {
            try
            {
                await _inventory.UpdateInventoryCard(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("UpdateInventoryCard", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateInventoryCardBatch([FromBody] List<InventoryCardDto> batch)
        {
            throw new NotImplementedException();
        }

        //Delete
        [HttpGet("[action]")]
        public async Task<ActionResult> DeleteInventoryCard(int id)
        {
            try
            {
                await _inventory.DeleteInventoryCard(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("DeleteInventoryCard", ex));
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> DeleteInventoryCardBatch(List<int> batchIDs)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Search

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


        //What if this took an int, instead of a name
        //The int could be any CardId, and the method would load all cards with the same name
        [HttpGet("[action]")]
        //public async Task<ActionResult<InventoryDetailDto>> GetInventoryDetail(string name)
        public async Task<ActionResult<InventoryDetailDto>> GetInventoryDetail(int cardId)
        {
            try
            {
                throw new NotImplementedException();
                //InventoryDetailDto result = await _inventory.GetInventoryDetailByName(name);
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, FormatExceptionMessage("GetByName", ex));
            }
        }

        #endregion

        #region Collection Builder

        //TODO - define param
        //TODO - define return

        [HttpGet("[action]")]
        public async Task<ActionResult> GetCollectionBuilderSuggestions()
        {
            throw new NotImplementedException();
        }

        //TODO - define param
        //should just return Ok()
        [HttpPost("[action]")]
        public async Task<ActionResult> HideCollectionBuilderSuggestion()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Trimming Tips

        //TODO - define param
        //TODO - define return
        [HttpGet("[action]")]
        public async Task<ActionResult> GetTrimmingTips()
        {
            throw new NotImplementedException();
        }

        //TODO - define param
        //Returns OK()
        [HttpPost("[action]")]
        public async Task<ActionResult> HideTrimmingTip()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Import

        //Validate Carpentry Import
        //TODO - define param
        //TODO - define return
        [HttpPost("[action]")]
        public async Task<ActionResult> ValidateCarpentryImport()
        {
            throw new NotImplementedException();
        }

        //Add Validated Carpentry Import
        //TODO - define param
        //Returns OK()
        [HttpPost("[action]")]
        public async Task<ActionResult> AddValidatedCarpentryImport()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Export

        //ExportInventoryBackup
        [HttpGet("[action]")]
        public async Task<ActionResult> ExportInventoryBackup()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
