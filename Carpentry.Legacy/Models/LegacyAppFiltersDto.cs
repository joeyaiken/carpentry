using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Legacy.Models
{
    public class LegacyAppFiltersDto
    {
        [JsonProperty("sets")]
        public List<LegacyFilterOptionDto> Sets { get; set; }

        [JsonProperty("types")]
        public List<LegacyFilterOptionDto> Types { get; set; }

        [JsonProperty("formats")]
        public List<LegacyFilterOptionDto> Formats { get; set; }

        [JsonProperty("colors")]
        public List<LegacyFilterOptionDto> ManaColors { get; set; }

        [JsonProperty("rarities")]
        public List<LegacyFilterOptionDto> Rarities { get; set; }

        [JsonProperty("statuses")]
        public List<LegacyFilterOptionDto> Statuses { get; set; }
    }
}
