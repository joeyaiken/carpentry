﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Logic.Models.Backups
{
    public class BackupDeck
    {
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

        [JsonProperty("c")]
        public List<BackupDeckCard> Cards { get; set; }
    }
}
