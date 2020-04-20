using Newtonsoft.Json;

namespace Carpentry.Service.Models
{
    public class NameSearchQueryParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("exclusive")]
        public bool Exclusive { get; set; }
    }
}
