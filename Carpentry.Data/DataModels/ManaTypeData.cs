using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    //Mana Type
    public class ManaTypeData
    {
        //public int Id { get; set; }
        public char Id { get; set; }
        public string Name { get; set; }
        //public char Abbreviation { get; set; }

        //Associations

        //CardColorIdentity -- ManaType
        public List<CardColorIdentityData> CardColorIdentities { get; set; }

        //cards whose colors contain this mana type
        public List<CardColorData> CardColors { get; set; }
    }

}
