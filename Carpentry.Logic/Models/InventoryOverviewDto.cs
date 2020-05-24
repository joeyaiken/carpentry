using Newtonsoft.Json;

namespace Carpentry.Logic.Models
{
    // TODO : Rename to CardOverviewDto 
    //  but actually, I think I may want a specific
    //inventory overview | inventory detail | deck overview | deck detail | Card Search Overview/Search Result Overview | Search Detail
    public class InventoryOverviewDto 
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("cost")]
        public string Cost { get; set; }
        
        [JsonProperty("cmc")]
        public int? Cmc { get; set; }
        
        [JsonProperty("img")]
        public string Img { get; set; }
        
        [JsonProperty("count")]
        public int Count { get; set; }

        //category / status / group
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("price")]
        public decimal? Price { get; set; }
        [JsonProperty("isFoil")]
        public bool? IsFoil { get; set; }
        [JsonProperty("variant")]
        public string Variant { get; set; }
    }
}
