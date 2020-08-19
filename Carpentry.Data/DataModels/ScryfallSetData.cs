using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    //ScryfallSet
    public class ScryfallSetData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime ReleasedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string SetType { get; set; }
        public int? CardCount { get; set; }
        public bool? Digital { get; set; }
        public bool? NonfoilOnly { get; set; }
        public bool? FoilOnly { get; set; }
        public string CardTokens { get; set; }
        public string SetCards { get; set; }
        public string PremiumCards { get; set; }
    }
}
