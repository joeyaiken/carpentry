using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarpentryMigrationTool.Models
{
    public class InventoryTextBackupDto
    {
        [JsonProperty("sets")]
        public List<string> SetCodes { get; set; }

        [JsonProperty("cards")]
        public List<InventoryCardBackup> Cards { get; set; }
    }
}
