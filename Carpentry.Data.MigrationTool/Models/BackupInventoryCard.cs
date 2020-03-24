using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.MigrationTool.Models
{
    class BackupInventoryCard
    {
        [JsonProperty("i")]
        public int MultiverseId { get; set; }

        [JsonProperty("s")]
        public int InventoryCardStatusId { get; set; }

        [JsonProperty("v")]
        public string VariantName { get; set; }

        [JsonProperty("f")]
        public bool IsFoil { get; set; }

        [JsonProperty("d")]
        public List<BackupDeckCard> DeckCards { get; set; }
    }
}
