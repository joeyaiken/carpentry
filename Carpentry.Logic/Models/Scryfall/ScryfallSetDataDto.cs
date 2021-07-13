using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models.Scryfall
{
    //This is used as a DTO to represent a 'set' in the scryfall repo
    //It contains some parsed props from the scryfall object, as well as properties for actual cards
    //  (It's what's retrieved when a set detail is requested)

    //?? Should it be refactored to hold a 'ScryfallSetOverview' and the relevant cards?
    public class ScryfallSetDataDto 
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string ReleaseDate { get; set; }

        public List<JToken> CardTokens { get; set; }

        public List<ScryfallMagicCard> SetCards { get; set; }

        public List<ScryfallMagicCard> PremiumCards { get; set; }
    }
}