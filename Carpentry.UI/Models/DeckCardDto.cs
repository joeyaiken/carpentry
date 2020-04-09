using Carpentry.Logic.Models;
using Newtonsoft.Json;

namespace Carpentry.UI.Models
{
    //TODO - Figure out if this is used, I'd like to have a version that doesn't include an inventory card
    public class DeckCardDto
    {
        public DeckCardDto()
        {
        }

        public DeckCardDto(DeckCard data)
        {
            Id = data.Id;
            DeckId = data.DeckId;
            CategoryId = data.CategoryId;
            InventoryCard = new InventoryCardDto(data.InventoryCard);
        }

        public DeckCard ToModel()
        {
            DeckCard deckCard = new DeckCard
            {
                InventoryCard = InventoryCard.ToModel(),
                CategoryId = CategoryId,
                DeckId = DeckId,
                Id = Id,
            };
            return deckCard;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deckId")]
        public int DeckId { get; set; }

        [JsonProperty("categoryId")]
        public char? CategoryId { get; set; }

        [JsonProperty("inventoryCard")]
        public InventoryCardDto InventoryCard { get; set; }
    }
}
