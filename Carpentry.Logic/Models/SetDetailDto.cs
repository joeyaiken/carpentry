using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class SetDetailDto
    {
        //code
        public string Code { get; set; }
        //name
        public string Name { get; set; }
        //Data Last Updated
        public DateTime? DataLastUpdated { get; set; }
        //Scry Last Updated
        public DateTime? ScryLastUpdated { get; set; }
        //Owned cards / card count / tracked count
        public int InventoryCardCount { get; set; }
    }
}
