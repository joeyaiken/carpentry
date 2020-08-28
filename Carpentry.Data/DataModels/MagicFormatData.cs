using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class MagicFormatData
    {
        [Key]
        public int FormatId { get; set; }
        public string Name { get; set; }

        //Associations
        public virtual ICollection<CardLegalityData> LegalCards { get; set; }
        public virtual ICollection<DeckData> Decks { get; set; }
    }

}
