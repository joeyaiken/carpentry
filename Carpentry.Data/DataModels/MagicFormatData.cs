﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class MagicFormatData
    {
        //id
        public int Id { get; set; }

        //name
        public string Name { get; set; }

        //Associations
        //card legalities
        public virtual ICollection<CardLegalityData> LegalCards { get; set; }
        public virtual ICollection<DeckData> Decks { get; set; }
    }

}
