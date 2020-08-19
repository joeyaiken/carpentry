using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class InventoryCardStatusData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }

        //Associations
        public virtual ICollection<InventoryCardData> Cards { get; set; }
    }

}
