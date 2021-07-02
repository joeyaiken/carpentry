using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Carpentry.CarpentryData.Models
{
    public class InventoryCardData
    {
        [Key]
        public int InventoryCardId { get; set; }
        public int CardId { get; set; }
        public int InventoryCardStatusId { get; set; }
        public bool IsFoil { get; set; }

        //Associations
        public virtual CardData Card { get; set; }
        public virtual ICollection<DeckCardData> DeckCards { get; set; }
        public virtual InventoryCardStatusData Status { get; set; }
    }
}
