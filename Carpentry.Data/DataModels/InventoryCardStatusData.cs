using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class InventoryCardStatusData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //Associations
        public List<InventoryCardData> Cards { get; set; }
    }

}
