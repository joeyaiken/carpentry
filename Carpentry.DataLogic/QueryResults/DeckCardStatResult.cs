using System.Collections.Generic;

namespace Carpentry.DataLogic.QueryResults
{
    public class DeckCardStatResult
    {
        //ID
        //Name?
        public decimal? Price { get; set; }

        public string Type { get; set; }

        public int? Cmc { get; set; }

        public List<string> ColorIdentity { get; set; }

        public char? CategoryId { get; set; }
    }
}
