using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class DeckOverviewDto
    {
        //[JsonProperty("id")]
        public int Id { get; set; }

        //[JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("format")]
        public string Format { get; set; }

        //[JsonProperty("colors")]
        public List<string> Colors { get; set; }

        //[JsonProperty("isValid")]
        public bool IsValid { get; set; }

        //[JsonProperty("validationIssues")]
        public string ValidationIssues { get; set; }

        //public DeckStatsDto Stats { get; set; }

        public bool IsDisassembled { get; set; }


        /*
         What info props would I want to put on here?
            IsValid
            IsAssembled
            IsDisassembled
         
         
         */
    }
}
