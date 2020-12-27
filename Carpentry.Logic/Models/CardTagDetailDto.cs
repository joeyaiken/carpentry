using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class CardTagDetailDto
    {
        public CardTagDetailDto()
        {
            ExistingTags = new List<CardTag>();
            TagSuggestions = new List<string>();
        }

        public int CardId { get; set; }
        public string CardName { get; set; }
        public List<CardTag> ExistingTags { get; set; }
        public List<string> TagSuggestions {get;set;}
    }


    public class CardTag
    {
        public int CardTagId { get; set; }
        public string Tag { get; set; }
    }
}
