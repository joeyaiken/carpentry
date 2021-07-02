using System.ComponentModel.DataAnnotations;

namespace Carpentry.CarpentryData.Models
{
    public class DeckCardTagData
    {
        [Key]
        public int DeckCardTagId { get; set; }
        public int DeckId { get; set; }
        public string CardName { get; set; }
        //TODO - figure out how to set max length of 100, or stop caring about maxlength here
        public string Description { get; set; }

        //Associations
        public virtual DeckData Deck { get; set; }
    }
}
