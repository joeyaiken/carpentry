using System.Collections.Generic;

namespace Carpentry.Legacy.Models
{
    public class CardSearchQueryParameter
    {
        public string Text { get; set; }
        public string Set { get; set; }
        public string Type { get; set; }
        public List<string> ColorIdentity { get; set; }
        public bool ExclusiveColorFilters { get; set; }
        public bool MultiColorOnly { get; set; }
        public List<string> Rarity { get; set; }
        public bool ExcludeUnowned { get; set; }
        public string SearchGroup { get; set; }
    }
}
