using Newtonsoft.Json;

namespace CarpentryMigrationTool.Models
{
    public class InventoryCardBackup
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("mid")]
        public int MultiverseId { get; set; }

        [JsonProperty("foil")]
        public bool IsFoil { get; set; }
        
        [JsonProperty("sid")]
        public int StatusId { get; set; }
    }
}
