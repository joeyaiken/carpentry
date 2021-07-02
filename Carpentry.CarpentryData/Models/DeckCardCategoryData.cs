using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpentry.CarpentryData.Models
{
    public class DeckCardCategoryData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public char DeckCardCategoryId { get; set; }
        public string Name { get; set; }

        //Associations
        public virtual ICollection<DeckCardData> Cards { get; set; }
    }
}
