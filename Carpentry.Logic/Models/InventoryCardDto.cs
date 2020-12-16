using Newtonsoft.Json;

namespace Carpentry.Logic.Models
{
    public class InventoryCardDto
    {
        //public InventoryCardDto()
        //{
        //    DeckCards = new List<InventoryDeckCardDto>();
        //}


        //[JsonProperty("cardId")]
        

        //[JsonProperty("isFoil")]

        //Inventory Card
        public int Id { get; set; }
        public bool IsFoil { get; set; }
        public int StatusId { get; set; }

        //Card
        public int CardId { get; set; }
        public string Name { get; set; }
        public string Set { get; set; }
        public int CollectorNumber { get; set; }

        //Deck Card (can be null
        //Deck Card ID
        //Deck Id
        //Deck Card Category
        public int? DeckCardId { get; set; }
        public int? DeckId { get; set; }
        public string DeckName { get; set; }
        public char? DeckCardCategory { get; set; }

        //[JsonProperty("deckCards")]
        //public InventoryDeckCardDto[] DeckCards { get; set; }

        //[JsonProperty("id")]
        

        //[JsonProperty("name")]

        //[JsonProperty("set")]
        

        // "is this in a deck? " / "Deck Id"
        //  Deck Card Category
        //  OR, just a DeckCard {id, deckId, category}
        //  


        //Should this be "deck cards" instead of "deck card IDs"
        //[JsonProperty("deckCardIds")]
        //public List<int> DeckCardIds { get; set; }


    }
}
