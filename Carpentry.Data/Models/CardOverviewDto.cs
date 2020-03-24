using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.Models
{
    public class CardOverviewDto
    {
        public int MultiverseId { get; set; }
        
        public string Name { get; set; }

        public string Type { get; set; }

        public string Cost { get; set; }

        public string Img { get; set; }

        public int Count { get; set; }
    }
}
