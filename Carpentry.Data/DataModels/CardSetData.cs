﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    //Card Set
    public class CardSetData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsTracked { get; set; }

        public DateTime? LastUpdated { get; set; }

        //Associations

        //Card -- Set
        public virtual ICollection<CardData> Cards { get; set; }
    }

}
