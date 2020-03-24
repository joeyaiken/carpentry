using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarpentryMigrationTool.Models
{
    public class DeckBackup
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("W")]
        public int BasicW { get; set; }

        [JsonProperty("U")]
        public int BasicU { get; set; }

        [JsonProperty("B")]
        public int BasicB { get; set; }

        [JsonProperty("R")]
        public int BasicR { get; set; }

        [JsonProperty("G")]
        public int BasicG { get; set; }
    }
}
