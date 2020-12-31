using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Logic.Models
{
    public class DeckStatsDto
    {
        public int TotalCount { get; set; }
        public Dictionary<string, int> TypeCounts { get; set; }
        public Dictionary<string, int> CostCounts { get; set; }
        public Dictionary<string, int> TagCounts { get; set; }
        public decimal TotalCost { get; set; }
        public List<string> ColorIdentity { get; set; }
    }
}
