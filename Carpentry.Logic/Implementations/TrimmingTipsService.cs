using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class TrimmingTipsService : ITrimmingTipsService
    {

        public TrimmingTipsService()
        {
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
            var totalUsedCardsToKeep = 10;
            var totalUnusedCardsToKeep = 6;



            




            //get a collection of cards used in 0 decks, ordered by count
            //var query = _


            //get 
















            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task HideTrimmingTip(InventoryOverviewDto dto)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }


        
    }
}
