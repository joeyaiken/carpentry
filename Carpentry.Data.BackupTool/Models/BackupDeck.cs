using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.BackupTool.Models
{
    class BackupDeck
    {
        [JsonProperty("id")]
        public int ExportId { get; set; }

        [JsonProperty("n")]
        public string Name { get; set; }

        [JsonProperty("f")]
        public string Format { get; set; }

        [JsonProperty("no")]
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
