﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class CardLegalityData
    {
        [Key]
        public int CardLegalityId { get; set; }
        public int CardId { get; set; }
        public int FormatId { get; set; }

        //Associations
        public virtual CardData Card { get; set; }
        public virtual MagicFormatData Format { get; set; }
    }
}
