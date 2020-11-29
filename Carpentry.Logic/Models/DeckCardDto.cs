using Newtonsoft.Json;

namespace Carpentry.Logic.Models
{
    //TODO - Figure out if this is used, I'd like to have a version that doesn't include an inventory card
    public class DeckCardDto
    {
        //deck card
        public int Id { get; set; }
        public int DeckId { get; set; }
        public string CardName { get; set; }
        public char? CategoryId { get; set; }

        
        //inventory card
        public int? InventoryCardId { get; set; }
        public int CardId { get; set; }
        public bool IsFoil { get; set; }
        public int InventoryCardStatusId { get; set; }
    }
}
