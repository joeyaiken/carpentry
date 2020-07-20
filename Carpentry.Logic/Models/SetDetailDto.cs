using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class SetDetailDto
    {
        public int SetId { get; set; }
        //code
        public string Code { get; set; }
        //name
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        //Data Last Updated
        public DateTime? DataLastUpdated { get; set; }
        
        //Scry Last Updated
        public DateTime? ScryLastUpdated { get; set; }
        //Owned cards / card count / tracked count

        public bool IsTracked { get; set; }

        public int? InventoryCount { get; set; }

        public int? CollectedCount { get; set; }

        public int? TotalCount { get; set; }



        //ownedCount: number;
        //collectedCount: number;
        //isTracked: boolean;
    }
}
