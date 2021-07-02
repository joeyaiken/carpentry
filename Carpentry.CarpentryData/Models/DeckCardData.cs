using System.ComponentModel.DataAnnotations;

namespace Carpentry.CarpentryData.Models
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
