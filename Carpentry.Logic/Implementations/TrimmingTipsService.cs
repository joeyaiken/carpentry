using Carpentry.Data.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class TrimmingTipsService : ITrimmingTipsService
    {
        private readonly IInventoryDataRepo _inventoryRepo;

        public TrimmingTipsService(IInventoryDataRepo inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;

            //how does this query cards by name? What data repo does it need?
        }

        /// <summary>
        /// Returns a list of trimming tips suggestions
        /// </summary>
        /// <remarks>
        /// Here are the current reasons a card would be recommended to be trimmed
        /// Cards used in 0 decks, where I own more than X
        /// Cards used in a deck, but I still have many copies of not in a deck
        /// </remarks>
        /// <returns></returns>
        public async Task<List<InventoryOverviewDto>> GetTrimmingTips()
        {

            //For each card, by print, I want to know
            //  # of owned cards by print
            //  # of copies in a deck, in ANY print
            //  # of copies, any print

            //Then...
            //  If 0 copies are in any deck, recomend trimming down to [6] total copies?
            //  If 1+ copies are in any deck, recomend trimming down to...[10] copies of this print? really?
            // w/e, as long as I trim down things
            


            //var totalUsedCardsToKeep = 10;
            //var totalUnusedCardsToKeep = 6;



            var trimmingTips = await _inventoryRepo.GetTrimmingTips();

            var totalTrimCount = await _inventoryRepo.GetTotalTrimCount();

            var result = trimmingTips.Select(t => new InventoryOverviewDto()
            {
                Category = null,
                Cmc = null,
                Cost = null,
                //Count = null,
                Description = null,
                Id = t.CardId,
                Img = null,
                IsFoil = null,
                Name = t.Name,
                Price = t.Price,
                Type = null,
                Variant = null,

            }).ToList();



            //get a collection of cards used in 0 decks, ordered by count
            //var query = _


            //get 














            return result;

            //await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        public async Task HideTrimmingTip(InventoryOverviewDto dto)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }


        
    }
}
