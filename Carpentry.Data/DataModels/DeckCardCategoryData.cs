using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class DeckCardCategoryData
    {
        //ID
        public char Id { get; set; }

        public string Name { get; set; }

        //Associations
        public List<DeckCardData> Cards { get; set; }
    }

}
