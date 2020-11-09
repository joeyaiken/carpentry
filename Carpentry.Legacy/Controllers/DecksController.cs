using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Legacy.Models;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.Legacy.Controllers
{
    [Route("api/[controller]")]
    public class DecksController : ControllerBase
    {
        
        public DecksController() { }

        /// <summary>
        /// This method just ensures the controller can start correctly (catches DI issues)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Online");
        }

        //decks/add
        //- add a deck
        [HttpPost("[action]")]
        public async Task<ActionResult<int>> Add([FromBody] LegacyDeckPropertiesDto deckProps)
        {
            var result = 1;
            return await Task.FromResult(Ok(result));
        }

        //decks/update
        //- update properties of a deck
        [HttpPost("[action]")]
        public async Task<ActionResult> Update([FromBody] LegacyDeckPropertiesDto deckProps)
        {
            return await Task.FromResult(Ok());
        }

        //decks/delete
        //- delete a deck
        [HttpGet("[action]")]
        public async Task<ActionResult> Delete(int deckId)
        {
            return await Task.FromResult(Ok());
        }

        //decks/Search
        //- get a list of deck properties & stats
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<DeckOverviewDto>>> Search()
        {
            var result = new List<DeckOverviewDto>()
            {
                new DeckOverviewDto()
                {
                    Name = "Test Deck",
                    Colors = new List<string>(){ "W","U","B","R","G"}

                }
            };
            return await Task.FromResult(Ok(result));
        }

        //decks/Get
        //- get a deck (with cards)
        [HttpGet("[action]")]
        public async Task<ActionResult<DeckDetailDto>> Get(int deckId)
        {
            var result = new DeckDetailDto()
            {
                CardOverviews = new List<DeckCardOverview>()
                {
                    
                },
                Cards = new List<DeckCard>()
                {

                },
                Props = new DeckPropertiesDto(),
                Stats = new DeckStatsDto()
                {
                    ColorIdentity = new List<string>(),
                    CostCounts = new Dictionary<string, int>(),
                    TotalCost = 0,
                    TotalCount = 0,
                    TypeCounts = new Dictionary<string, int>(),
                },
            };
            return await Task.FromResult(result);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> AddCard([FromBody] LegacyDeckCardDto dto)
        {
            return await Task.FromResult(Ok());
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateCard([FromBody] LegacyDeckCardDto card)
        {
            return await Task.FromResult(Ok());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> RemoveCard(int id)
        {
            return await Task.FromResult(Ok());
        }
    }
}
