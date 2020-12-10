using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models.Backups
{
    //Minimalistic class representing a deck card
    //(wrong) //Doesn't contain personal id, or inventory id, because it will be part of a backup inventory card
    public class BackupDeckCard
    {
        [JsonProperty("n")]
        public string Name { get; set; }

        [JsonProperty("c")]
        public char? Category { get; set; }

        [JsonProperty("i")]
        public BackupInventoryCard InventoryCard { get; set; }
    }
}
