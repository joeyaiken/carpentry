using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class DeckCardCategoryData
    {
        //ID
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public char Id { get; set; }

        public string Name { get; set; }

        //Associations
        public List<DeckCardData> Cards { get; set; }
    }

}
