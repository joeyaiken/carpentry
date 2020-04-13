using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class MagicCard
    {
        public int? Cmc { get; set; }
        public List<string> ColorIdentity { get; set; }
        public List<string> Colors { get; set; }
        public string ManaCost { get; set; }
        public int MultiverseId { get; set; }
        public string Name { get; set; }
        //TODO - This really should be a different class instead of two weird dicts
        public Dictionary<string, decimal?> Prices { get; set; }
        public Dictionary<string, string> Variants { get; set; }
        public List<string> Legalities { get; set; }
        public string Rarity { get; set; }
        public string Set { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
    }
}
