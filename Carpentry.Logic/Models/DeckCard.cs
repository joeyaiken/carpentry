using Newtonsoft.Json;

namespace Carpentry.Logic.Models
{
    //This is used by the Deck Detail DTO 
    public class DeckCard
    {
        //set
        //IsFoil
        //Variant
        //Category
        //MID


        [JsonProperty("id")]
        public int Id { get; set; }

        //[JsonProperty("multiverseId")]
        //public int MultiverseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("isFoil")]
        public bool IsFoil { get; set; }

        //[JsonProperty("variantName")]
        //public string VariantType { get; set; }

        public int? CollectorNumber { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }







        //[JsonProperty("deckId")]
        //public int DeckId { get; set; }

        //[JsonProperty("categoryId")]
        //public char? CategoryId { get; set; }

        //[JsonProperty("inventoryCard")]
        //public InventoryCard InventoryCard { get; set; }
    }
}
