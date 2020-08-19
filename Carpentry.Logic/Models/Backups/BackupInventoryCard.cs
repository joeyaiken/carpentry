using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Logic.Models.Backups
{
    class BackupInventoryCard
    {
        [JsonProperty("s")]
        public string SetCode { get; set; }

        [JsonProperty("n")]
        public int CollectorNumber { get; set; }

        [JsonProperty("f")]
        public bool IsFoil { get; set; }

        [JsonProperty("i")]
        public int InventoryCardStatusId { get; set; }

        [JsonProperty("d")]
        public List<BackupDeckCard> DeckCards { get; set; }
    }
}
