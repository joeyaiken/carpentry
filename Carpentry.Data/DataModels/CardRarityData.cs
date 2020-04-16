using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Carpentry.Data.DataModels
{
    //Card Rarity
    public class CardRarityData
    {
        //char or int ??
        //public int Id { get; set; }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public char Id { get; set; }

        public string Name { get; set; }

        //Associations

        //Card -- Rarity
        public List<CardData> Cards { get; set; }
    }

}
