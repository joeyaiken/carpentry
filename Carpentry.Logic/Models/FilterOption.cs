﻿using Newtonsoft.Json;

namespace Carpentry.UI.Models
{
    public class FilterOption
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
