using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.QueryResults
{
    public class DeckCardStatResult
    {
        //ID
        //Name?
        public decimal? Price { get; set; }

        public string Type { get; set; }

        public int? Cmc { get; set; }

        public List<char> ColorIdentity { get; set; }

        public char? CategoryId { get; set; }
    }
}
