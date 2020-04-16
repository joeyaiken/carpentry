using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Implementations
{
    public class InventoryDataRepo : IInventoryDataRepo
    {
        private readonly CarpentryDataContext _cardContext;

        public InventoryDataRepo(CarpentryDataContext cardContext)
        {
            _cardContext = cardContext;
        }

        public async Task<InventoryCardData> GetInventoryCardById(int inventoryCardId)
        {
            var result = await _cardContext.InventoryCards.Where(x => x.Id == inventoryCardId).FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// Adds a new card to the inventory
        /// Does not handle adding deck cards
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> AddInventoryCard(InventoryCardData cardToAdd)
        {
            await _cardContext.InventoryCards.AddAsync(cardToAdd);
            await _cardContext.SaveChangesAsync();

            return cardToAdd.Id;
        }

        public async Task AddInventoryCardBatch(List<InventoryCardData> cardBatch)
        {
            await _cardContext.InventoryCards.AddRangeAsync(cardBatch);
            await _cardContext.SaveChangesAsync();
        }


        /// <summary>
        /// Updates an inventory card
        /// In theory, the only fieds I'd practically want to update would be Status and maybe IsFoil??
        /// This one might need to wait until variants are handled better...
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateInventoryCard(InventoryCardData cardToUpdate)
        {
            //todo - actually check if exists?
            _cardContext.InventoryCards.Update(cardToUpdate);
            await _cardContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a card from the inventory
        /// Can only delete cards that don't belong to a deck
        /// </summary>
        /// <param name="id">Id of card to delete</param>
        /// <returns></returns>
        public async Task DeleteInventoryCard(int id)
        {
            var deckCardsReferencingThisCard = _cardContext.DeckCards.Where(x => x.DeckId == id).Count();

            if (deckCardsReferencingThisCard > 0)
            {
                throw new Exception("Cannot delete a card that's currently in a deck");
            }

            var cardToRemove = _cardContext.InventoryCards.First(x => x.Id == id);

            _cardContext.InventoryCards.Remove(cardToRemove);

            await _cardContext.SaveChangesAsync();
        }

        public async Task<bool> DoInventoryCardsExist()
        {
            InventoryCardData firstCard = await _cardContext.InventoryCards.FirstOrDefaultAsync();
            return (firstCard != null);
        }
    }
}
