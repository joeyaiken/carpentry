using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models.Scryfall
{
    public class ScryfallSetOverviewDto
    {

        //"code": "2xm",
        public string Code { get; set; }

        //"name": "Double Masters",
        public string Name { get; set; }

        //"released_at": "2020-08-07",
        public string ReleasedAtString { get; set; }

        //"set_type": "masters",
        public string SetType { get; set; }        

        //"card_count": 10,
        public int CardCount { get; set; }
        //"digital": false,
        public bool Digital { get; set; }
        //"nonfoil_only": false,
        public bool NonfoilOnly { get; set; }
        //"foil_only": false,
        public bool FoilOnly { get; set; }
        //"icon_svg_uri": "https://img.scryfall.com/sets/2xm.svg?1593403200"

    }
}
