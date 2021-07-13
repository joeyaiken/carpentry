using System;
using System.ComponentModel.DataAnnotations;

namespace Carpentry.ScryfallData.Models
{
    //DATABASE object representing a scryfall set
    //Should contain a string of the set detail, along with a string of all cards in the set
    public class ScryfallSetData
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string SetTokenString { get; set; }
        public string CardTokensString { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
