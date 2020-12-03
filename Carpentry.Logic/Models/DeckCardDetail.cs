using Newtonsoft.Json;

namespace Carpentry.Logic.Models
{
    //This is used by the Deck Detail DTO 
    public class DeckCardDetail
    {
        //set
        //IsFoil
        //Variant
        //Category
        //MID


        public int Id { get; set; }
        public int OverviewId { get; set; }

        //[JsonProperty("multiverseId")]
        //public int MultiverseId { get; set; }

        public string Name { get; set; }

        public string Set { get; set; }

        public bool IsFoil { get; set; }

        //[JsonProperty("variantName")]
        //public string VariantType { get; set; }

        public int? CollectorNumber { get; set; }

        public string Category { get; set; }







        //[JsonProperty("deckId")]
        //public int DeckId { get; set; }

        //[JsonProperty("categoryId")]
        //public char? CategoryId { get; set; }

        //[JsonProperty("inventoryCard")]
        //public InventoryCard InventoryCard { get; set; }
    }
}
