using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Legacy.Models
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
