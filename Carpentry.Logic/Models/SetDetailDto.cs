using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class SetDetailDto
    {
        public int SetId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime? DataLastUpdated { get; set; }
        
        //Owned cards / card count / tracked count
        public bool IsTracked { get; set; }
        public int? InventoryCount { get; set; }
        public int? CollectedCount { get; set; }
        public int? TotalCount { get; set; }
    }
}
