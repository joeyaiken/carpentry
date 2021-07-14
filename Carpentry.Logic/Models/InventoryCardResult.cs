namespace Carpentry.Logic.Models
{
    public class InventoryCardResult
    {
        public int Id { get; set; }
        public bool IsFoil { get; set; }
        public int InventoryCardStatusId { get; set; }

        public int CardId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Set { get; set; }
        public int CollectorNumber { get; set; }

        public int? DeckCardId { get; set; }
        public int? DeckId { get; set; }
        public string DeckName { get; set; }
        public char? DeckCardCategory { get; set; }





        //Should this be "deck cards" instead of "deck card IDs"
        //[JsonProperty("deckCardIds")]
        //public List<int> DeckCardIds { get; set; }

        //public List<DeckCardResult> DeckCards { get; set; }
    }
}
