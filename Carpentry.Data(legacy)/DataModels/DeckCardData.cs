using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class DeckCardData
    {
        [Key]
        public int DeckCardId { get; set; }
        public int DeckId { get; set; }
        public string CardName { get; set; }
        public int? InventoryCardId { get; set; }
        public char? CategoryId { get; set; }

        //Associations
        public virtual InventoryCardData InventoryCard { get; set; }
        public virtual DeckData Deck { get; set; }
        public virtual DeckCardCategoryData Category { get; set; }

    }

}
