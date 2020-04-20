using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Service.Models
{
    public class InventoryQueryParameter
    {
        [JsonProperty("groupBy")]
        public string GroupBy { get; set; }

        [JsonProperty("colors")]
        public List<string> Colors { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rarity")]
        public List<string> Rarity { get; set; }

        //[JsonProperty("colorIdentity")]
        //public List<string> ColorIdentity { get; set; }

        //need an "exclusive" boolean filter for colors
        [JsonProperty("exclusiveColorFilters")]
        public bool ExclusiveColorFilters { get; set; }

        //need a "multi-color only" boolean filter
        [JsonProperty("multiColorOnly")]
        public bool MultiColorOnly { get; set; }

        //[JsonProperty("sets")]
        //public List<string> Sets { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("skip")]
        public int Skip { get; set; }

        [JsonProperty("take")]
        public int Take { get; set; }

        //One or list? How about 1 for now
        [JsonProperty("format")]
        public string Format { get; set; }

        //other things to add?
        //Format / Legality



        //Sort ?
        [JsonProperty("sort")]
        public string Sort { get; set; }



        [JsonProperty("minCount")]
        public int MinCount { get; set; }

        [JsonProperty("maxCount")]
        public int MaxCount { get; set; }

        [JsonProperty("statusId")]
        public int StatusId { get; set; }
    }
}
