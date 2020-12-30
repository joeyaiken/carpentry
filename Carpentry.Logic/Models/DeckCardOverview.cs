using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class DeckCardOverview
    {
        public DeckCardOverview()
        {
            Tags = new List<string>();
            Details = new List<DeckCardDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Cost { get; set; }
        public int? Cmc { get; set; }
        public string Img { get; set; }
        public int Count { get; set; }
        public string Category { get; set; }
        public int CardId { get; set; }

        public List<string> Tags { get; set; }

        public List<DeckCardDetail> Details { get; set; }
    }
}
