using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.Models;
//using Carpentry.Data.Models;
//using Carpentry.Interfaces;
using Carpentry.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Controllers
{
    [Route("api/[controller]")]
    public class DecksController : ControllerBase
    {
        private string FormatExceptionMessage(string functionName, Exception ex)
        {
            return $"An error occured when processing the {functionName} method of the Decks controller: {ex.Message}";
        }

        private readonly ICarpentryService _carpentry;

        public DecksController(ICarpentryService carpentry)
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

        ////decks/add
        ////- add a deck
        //[HttpPost("[action]")]
        //public async Task<ActionResult<int>> Add([FromBody] DeckProperties deckProps)
        //{
        //    throw new NotImplementedException();
        //    //try
        //    //{
        //    //    int newDeckId = await _carpentry.AddDeck(deckProps);
        //    //    return Accepted(newDeckId);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return StatusCode(500, FormatExceptionMessage("Add", ex));
        //    //}
        //}

        ////decks/update
        ////- update properties of a deck
        //[HttpPost("[action]")]
        //public async Task<ActionResult> Update([FromBody] DeckProperties deckProps)
        //{
        //    throw new NotImplementedException();
        //    //try
        //    //{
        //    //    await _carpentry.UpdateDeck(deckProps);
        //    //    return Accepted();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return StatusCode(500, FormatExceptionMessage("Update", ex));
        //    //}
        //}

        ////decks/delete
        ////- delete a deck
        //[HttpGet("[action]")]
        //public async Task<ActionResult> Delete(int deckId)
        //{
        //    try
        //    {
        //        await _carpentry.DeleteDeck(deckId);
        //        return Accepted();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("Delete", ex));
        //    }
        //}

        ////decks/Search
        ////- get a list of deck properties & stats
        //[HttpPost("[action]")]
        //public async Task<ActionResult<IEnumerable<DeckProperties>>> Search()
        //{
        //    try
        //    {
        //        var results = await _carpentry.SearchDecks();
        //        return Ok(results);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("Search", ex));
        //    }
        //}

        ////decks/Get
        ////- get a deck (with cards)
        //[HttpGet("[action]")]
        //public async Task<ActionResult<DeckDto>> Get(int deckId)
        //{
        //    try
        //    {
        //        var results = await _carpentry.GetDeckDetail(deckId);
        //        return Ok(results);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("Get", ex));
        //    }
        //}


        //[HttpPost("[action]")]
        //public async Task<ActionResult> AddCard([FromBody] DeckCardDto dto)
        //{
        //    throw new NotImplementedException();
        //    //try
        //    //{
        //    //    await _carpentry.AddDeckCard(dto);
        //    //    return Accepted();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return StatusCode(500, FormatExceptionMessage("AddCard", ex));
        //    //}
        //}

        //[HttpPost("[action]")]
        //public async Task<ActionResult> UpdateCard([FromBody] DeckCardDto card)
        //{
        //    throw new NotImplementedException();
        //    //try
        //    //{
        //    //    await _carpentry.UpdateDeckCard(card);
        //    //    return Accepted();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return StatusCode(500, FormatExceptionMessage("UpdateCard", ex));
        //    //}
        //}

        //[HttpGet("[action]")]
        //public async Task<ActionResult> RemoveCard(int id)
        //{
        //    try
        //    {
        //        await _carpentry.DeleteDeckCard(id);
        //        return Accepted();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, FormatExceptionMessage("RemoveCard", ex));
        //    }
        //}
    }
}
