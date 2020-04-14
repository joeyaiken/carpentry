using Carpentry.Logic.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Carpentry.UI.Legacy.Models
{
    public class InventoryCardDto
    {
        public InventoryCardDto()
        {
            DeckCards = new List<InventoryDeckCardDto>();
        }

        public InventoryCardDto(InventoryCard model)
        {
            Id = model.Id;
            DeckCards = model.DeckCards.Select(x => new InventoryDeckCardDto(x)).ToList();
            InventoryCardStatusId = model.InventoryCardStatusId;
            IsFoil = model.IsFoil;
            MultiverseId = model.MultiverseId;
            Name = model.Name;
            Set = model.Set;
            VariantType = model.VariantType;
        }

        public InventoryCard ToModel()
        {
            InventoryCard result = new InventoryCard
            {
                Id = Id,
                DeckCards = DeckCards.Select(x => x.ToModel()).ToList(),
                InventoryCardStatusId = InventoryCardStatusId,
                IsFoil = IsFoil,
                MultiverseId = MultiverseId,
                Name = Name,
                Set = Set,
                VariantType = VariantType,
            };
            return result;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("multiverseId")]
        public int MultiverseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("isFoil")]
        public bool IsFoil { get; set; }

        [JsonProperty("variantName")]
        public string VariantType { get; set; }

        [JsonProperty("statusId")]
        public int InventoryCardStatusId { get; set; }

        //Should this be "deck cards" instead of "deck card IDs"
        //[JsonProperty("deckCardIds")]
        //public List<int> DeckCardIds { get; set; }

        [JsonProperty("deckCards")]
        public List<InventoryDeckCardDto> DeckCards { get; set; }
    }
}
