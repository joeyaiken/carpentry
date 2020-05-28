using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class CardColorPairData
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public char ManaTypeId { get; set; }

        //Associations

        //CardColorIdentity -- ManaType
        public virtual ManaTypeData ManaType { get; set; }

        //Card -- CardColorIdentity
        public virtual CardData Card { get; set; }

    }

}
