using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class MagicFormatData
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Associations
        public virtual ICollection<CardLegalityData> LegalCards { get; set; }
        public virtual ICollection<DeckData> Decks { get; set; }
    }

}
