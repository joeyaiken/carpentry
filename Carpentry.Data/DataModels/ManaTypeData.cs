using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Carpentry.Data.DataModels
{
    //Mana Type
    public class ManaTypeData
    {
        //public int Id { get; set; }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public char Id { get; set; }
        public string Name { get; set; }
        //public char Abbreviation { get; set; }

        //Associations

        //CardColorIdentity -- ManaType
        public virtual ICollection<CardColorIdentityData> CardColorIdentities { get; set; }

        //cards whose colors contain this mana type
        public virtual ICollection<CardColorData> CardColors { get; set; }
    }

}
