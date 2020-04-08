using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models.Backups
{
    //Minimalistic class representing a deck card
    //Doesn't contain personal id, or inventory id, because it will be part of a backup inventory card
    class BackupDeckCard
    {
        [JsonProperty("d")]
        public int DeckId { get; set; }

        [JsonProperty("c")]
        public char? Category { get; set; }
    }
}
