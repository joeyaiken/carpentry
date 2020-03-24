using Carpentry.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Models
{
    public class FilterOptionDto
    {
        [JsonProperty("sets")]
        public List<FilterDescriptor> Sets { get; set; }

        [JsonProperty("types")]
        public List<FilterDescriptor> Types { get; set; }

        [JsonProperty("formats")]
        public List<FilterDescriptor> Formats { get; set; }

        [JsonProperty("colors")]
        public List<FilterDescriptor> ManaColors { get; set; }

        [JsonProperty("rarities")]
        public List<FilterDescriptor> Rarities { get; set; }
        
        [JsonProperty("statuses")]
        public List<FilterDescriptor> Statuses { get; set; }
    }
}
