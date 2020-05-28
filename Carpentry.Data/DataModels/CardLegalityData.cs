using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class CardLegalityData
    {
        //id
        public int Id { get; set; }

        //card id
        public int CardId { get; set; }

        //format id
        public int FormatId { get; set; }

        //Associations
        public virtual CardData Card { get; set; }
        public virtual MagicFormatData Format { get; set; }
    }

}
