using Newtonsoft.Json;

namespace CarpentryMigrationTool.Models
{
    public class DeckCardBackup
    {
        [JsonProperty("did")]
        public int DeckId { get; set; }

        [JsonProperty("iid")]
        public int InventoryCardId { get; set; }
    }
}
