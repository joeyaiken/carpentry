using Newtonsoft.Json;

namespace Carpentry.Data.Models
{
    public class NameSearchQueryParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("exclusive")]
        public bool Exclusive { get; set; }
    }
}
