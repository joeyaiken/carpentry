using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Logic.Models.Backups
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
