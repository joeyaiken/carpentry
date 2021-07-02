using System;
using System.ComponentModel.DataAnnotations;

namespace Carpentry.ScryfallData.Models
{
    public class ScryfallSetData
    {
        [Key]
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
