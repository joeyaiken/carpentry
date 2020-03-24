using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Data.Models
{
    //public class DeckStatCount 
    //{
    //    public string Name { get; set; }
    //    public int Count { get; set; }
    //}
    
    public class DeckStats
    {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    
        [JsonProperty("totalCost")]
        public decimal TotalCost { get; set; }
    
        [JsonProperty("typeCounts")]
        //public List<DeckStatCount> TypeCounts { get; set; }
        public Dictionary<string, int> TypeCounts { get; set; }

        [JsonProperty("costCounts")]
        //public List<DeckStatCount> CostCounts { get; set; }
        public Dictionary<string, int> CostCounts { get; set; }


        //Should this include color identity??
        public List<char> ColorIdentity { get; set; }
    }
}
