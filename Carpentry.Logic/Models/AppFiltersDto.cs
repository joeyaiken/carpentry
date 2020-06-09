using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Logic.Models
{
    public class AppFiltersDto
    {
        [JsonProperty("sets")]
        public List<FilterOption> Sets { get; set; }

        [JsonProperty("types")]
        public List<FilterOption> Types { get; set; }

        [JsonProperty("formats")]
        public List<FilterOption> Formats { get; set; }

        [JsonProperty("colors")]
        public List<FilterOption> Colors { get; set; }

        [JsonProperty("rarities")]
        public List<FilterOption> Rarities { get; set; }

        [JsonProperty("statuses")]
        public List<FilterOption> Statuses { get; set; }
    }
}
