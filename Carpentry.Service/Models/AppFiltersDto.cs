using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Service.Models
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
