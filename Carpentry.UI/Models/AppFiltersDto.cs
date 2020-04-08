using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Models
{
    public class AppFiltersDto
    {
        [JsonProperty("sets")]
        public List<FilterOptionDto> Sets { get; set; }

        [JsonProperty("types")]
        public List<FilterOptionDto> Types { get; set; }

        [JsonProperty("formats")]
        public List<FilterOptionDto> Formats { get; set; }

        [JsonProperty("colors")]
        public List<FilterOptionDto> ManaColors { get; set; }

        [JsonProperty("rarities")]
        public List<FilterOptionDto> Rarities { get; set; }

        [JsonProperty("statuses")]
        public List<FilterOptionDto> Statuses { get; set; }
    }
}
