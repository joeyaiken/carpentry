using Newtonsoft.Json;

namespace Carpentry.Logic.Models.Backups
{
    public class BackupInventoryCard
    {
        [JsonProperty("s")]
        public string SetCode { get; set; }

        [JsonProperty("n")]
        public string CollectorNumberStr { get; set; }

        [JsonProperty("f")]
        public bool IsFoil { get; set; }

        [JsonProperty("i")]
        public int InventoryCardStatusId { get; set; }
    }
}
