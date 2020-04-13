using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Data.LegacyModels
{
    public class InventoryQueryResult
    {
        public string Name { get; set; }

        public List<MagicCardDto> Cards { get; set; }

        public List<Card> Items { get; set; }

        //public MagicCardDto Data { get; set; }

        //public List<InventoryResultItem> Items { get; set; }
    }

    public class InventoryResultItem
    {
        public string Set { get; set; }

        public int Count { get; set; }

        public int CountFoil { get; set; }

        public decimal Price { get; set; }

        public decimal PriceFoil { get; set; }
    }
}
