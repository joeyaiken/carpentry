﻿using Newtonsoft.Json;

namespace Carpentry.Service.Models
{
    public class FilterOptionDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}