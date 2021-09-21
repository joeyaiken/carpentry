using System.Collections.Generic;

namespace Carpentry.Logic.Models.Search
{
    //public enum InventoryQueryGroupByOption
    //{
    //    name,
    //    unique,
    //    print,
    //}

    public class InventoryQueryParameter : CardSearchQueryParameterBase
    {
        public string GroupBy { get; set; } //options: name | print | unique
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Sort { get; set; }
        public bool SortDescending { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
    }
}
