using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class DeckCardData
    {
        public int Id { get; set; }

        public int DeckId { get; set; }

        public int InventoryCardId { get; set; }

        public char? CategoryId { get; set; }

        //Associations

        //DeckInventoryCard -- InventoryCard
        public InventoryCardData InventoryCard { get; set; }

        //Deck -- DeckInventoryCard
        public DeckData Deck { get; set; }
        //DeckCard - Status
        public DeckCardCategoryData Category { get; set; }

    }

}
