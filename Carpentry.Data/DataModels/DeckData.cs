using System.Collections.Generic;

namespace Carpentry.Data.DataModels
{
    public class DeckData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        //public string Format { get; set; } //Should eventually be a FK table
        public int MagicFormatId { get; set; }

        //Basic lands will still be tracked here (for now)
        public int BasicW { get; set; }

        public int BasicU { get; set; }

        public int BasicB { get; set; }

        public int BasicR { get; set; }

        public int BasicG { get; set; }

        //Associations

        //Deck -- DeckInventoryCard
        public virtual ICollection<DeckCardData> Cards { get; set; }

        //Deck -- MagicFormat
        public virtual MagicFormatData Format { get; set; }
    }
}
