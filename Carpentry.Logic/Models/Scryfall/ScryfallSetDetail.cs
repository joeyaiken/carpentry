using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models.Scryfall
{
    public class ScryfallSetDetail
    {
        public ScryfallSetOverview Overview { get; set; }
        public List<ScryfallMagicCard> Cards { get; set; }
    }
}
