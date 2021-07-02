using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpentry.CarpentryData.Models
{
    public class InventoryCardStatusData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CardStatusId { get; set; }
        public string Name { get; set; }

        //Associations
        public virtual ICollection<InventoryCardData> Cards { get; set; }
    }
}
