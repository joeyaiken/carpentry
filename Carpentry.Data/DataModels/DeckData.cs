using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Carpentry.Data.DataModels
{
    public class DeckData
    {
        [Key]
        public int DeckId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int MagicFormatId { get; set; }
        public int BasicW { get; set; }
        public int BasicU { get; set; }
        public int BasicB { get; set; }
        public int BasicR { get; set; }
        public int BasicG { get; set; }

        //Associations
        public virtual ICollection<DeckCardData> Cards { get; set; }
        public virtual MagicFormatData Format { get; set; }
        public virtual ICollection<DeckCardTagData> Tags { get; set; }
    }
}
