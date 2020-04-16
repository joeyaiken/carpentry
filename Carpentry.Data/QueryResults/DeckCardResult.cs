using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.QueryResults
{
    public class DeckCardResult
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public string DeckName { get; set; }
        public int InventoryCardId { get; set; }
        public string DeckCardCategory { get; set; }
    }
}
