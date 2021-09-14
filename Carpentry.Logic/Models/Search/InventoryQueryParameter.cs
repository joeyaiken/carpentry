using System.Collections.Generic;

namespace Carpentry.Logic.Models.Search
{
    //public enum InventoryQueryGroupByOption
    //{
    //    name,
    //    unique,
    //    print,
    //}

    public class InventoryQueryParameter
    {
        public string GroupBy { get; set; } //options: name | print | unique
        public List<string> Colors { get; set; }
        public string Type { get; set; }
        public List<string> Rarity { get; set; }
        public bool ExclusiveColorFilters { get; set; }
        public bool MultiColorOnly { get; set; }
        public string Set { get; set; }
        public string Text { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Sort { get; set; }
        public bool SortDescending { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
    }
}
