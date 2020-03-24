using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarpentryMigrationTool.Models
{
    public class DeckTextBackupDto
    {
        [JsonProperty("decks")]
        public List<DeckBackup> Decks { get; set; }

        //ATM there are no duplicates for cards => decks
        
        //What if InventoryCards stored a record of what decks they were in
        
        //Could have 1 less record type, 
        //I feel like I'm WAY over thinking this
        
        [JsonProperty("cards")]
        public List<DeckCardBackup> DeckCards { get; set; }
    }
}
