using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Logic.Models
{
    //uses json properties for saving to deck props
    public class DeckStatsDto
    {
        //[JsonProperty("TotalCount")]
        public int TotalCount { get; set; }
        //[JsonProperty("TypeCounts")]
        public Dictionary<string, int> TypeCounts { get; set; }
        //[JsonProperty("CostCounts")]
        public Dictionary<string, int> CostCounts { get; set; }
        //[JsonProperty("TagCounts")]
        public Dictionary<string, int> TagCounts { get; set; }
        //[JsonProperty("TotalCost")]
        public decimal TotalCost { get; set; }
        //[JsonProperty("ColorIdentity")]
        public List<string> ColorIdentity { get; set; }

        //isValid
        //[JsonProperty("IsValid")]
        public bool IsValid { get; set; }
        //validationIssues
        //public List<string> ValidationIssues { get; set; }
        //[JsonProperty("ValidationIssues")]
        public string ValidationIssues { get; set; }

        public bool IsDisassembled { get; set; }

    }
}
