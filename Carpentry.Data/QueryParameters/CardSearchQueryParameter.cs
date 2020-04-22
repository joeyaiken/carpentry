﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Data.QueryParameters
{
    public class CardSearchQueryParameter
    {
        [JsonProperty("set")]
        public string SetCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("colorIdentity")]
        public List<string> ColorIdentity { get; set; }

        //need an "exclusive" boolean filter for colors
        [JsonProperty("exclusiveColorFilters")]
        public bool ExclusiveColorFilters { get; set; }
        
        //need a "multi-color only" boolean filter
        [JsonProperty("multiColorOnly")]
        public bool MultiColorOnly { get; set; }
        
        [JsonProperty("rarity")]
        public List<string> Rarity { get; set; }
    }
}