using Carpentry.Logic.Models;
using Newtonsoft.Json;

namespace Carpentry.UI.Models
{
    public class FilterOptionDto
    {
        public FilterOptionDto(FilterOption model)
        {
            Name = model.Name;
            Value = model.Value;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
