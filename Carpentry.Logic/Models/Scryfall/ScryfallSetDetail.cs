using System.Collections.Generic;

namespace Carpentry.Logic.Models.Scryfall
{
    public class ScryfallSetDetail
    {
        public ScryfallSetOverview Overview { get; set; }
        public List<ScryfallMagicCard> Cards { get; set; }
    }
}
