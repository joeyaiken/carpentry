using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Carpentry.CarpentryData.Models
{
    public class CardSetData
    {
        [Key]
        public int SetId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsTracked { get; set; }
        public DateTime? LastUpdated { get; set; }

        //Associations
        public virtual ICollection<CardData> Cards { get; set; }
    }
}
