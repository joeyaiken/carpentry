using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Logic.Models;
using Carpentry.Logic.Interfaces;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using System.Linq;

namespace Carpentry.Service.Implementations
{
    public class DeckControllerService : IDeckControllerService
    {

        public IDataReferenceService _referenceService;

        public IDeckService _deckService;

        public DeckControllerService(
            IDataReferenceService referenceService,
            IDeckService deckService
            )
        {
            _referenceService = referenceService;
            _deckService = deckService;
        }
        public async Task<int> AddDeck(DeckPropertiesDto props)
        {
            DataReferenceValue<int> deckFormat = await _referenceService.GetMagicFormat(props.Format);

            DeckProperties deckModel = new DeckProperties()
            {
                Name = props.Name,
                FormatId = deckFormat.Id,
                Notes = props.Notes,

                BasicW = props.BasicW,
                BasicU = props.BasicU,
                BasicB = props.BasicB,
                BasicR = props.BasicR,
                BasicG = props.BasicG,
            };

            int newId = await _deckService.AddDeck(deckModel);

            return newId;
        }

        public async Task UpdateDeck(DeckPropertiesDto props)
        {
            DataReferenceValue<int> deckFormat = await _referenceService.GetMagicFormat(props.Format);

            DeckProperties deckModel = new DeckProperties()
            {
                Id = props.Id,
                Name = props.Name,
                FormatId = deckFormat.Id,
                Notes = props.Notes,

                BasicW = props.BasicW,
                BasicU = props.BasicU,
                BasicB = props.BasicB,
                BasicR = props.BasicR,
                BasicG = props.BasicG,
            };

            await _deckService.UpdateDeck(deckModel);
        }

        public async Task DeleteDeck(int deckId)
        {
            await _deckService.DeleteDeck(deckId);
        }

        public async Task<IEnumerable<DeckPropertiesDto>> GetDeckOverviews()
        {
            throw new NotImplementedException();
        }

        public async Task<DeckDetailDto> GetDeckDetail(int deckId)
        {
            throw new NotImplementedException();
        }

        public async Task AddDeckCard(DeckCardDto dto)
        {
            DeckCard mappedCard = MapDeckCardDto(dto);

            await _deckService.AddDeckCard(mappedCard);
        }

        public async Task AddDeckCardBatch(IEnumerable<DeckCardDto> dto)
        {
            List<DeckCard> mappedCards = dto.Select(x => MapDeckCardDto(x)).ToList();

            await _deckService.AddDeckCardBatch(mappedCards);
        }

        public async Task UpdateDeckCard(DeckCardDto dto)
        {
            DeckCard mappedCard = MapDeckCardDto(dto);

            await _deckService.UpdateDeckCard(mappedCard);
        }

        public async Task DeleteDeckCard(int deckCardId)
        {
            await _deckService.DeleteDeckCard(deckCardId);
        }

        #region private

        private static DeckCard MapDeckCardDto(DeckCardDto dto)
        {
            if(dto == null)
            {
                return null;
            }

            DeckCard result = new DeckCard()
            {
                CategoryId = dto.CategoryId,
                DeckId = dto.DeckId,
                InventoryCard = MapInventoryCardDto(dto.InventoryCard),
            };

            return result;
        }

        private static InventoryCard MapInventoryCardDto(InventoryCardDto dto)
        {
            if(dto == null)
            {
                return null;
            }

            InventoryCard result = new InventoryCard()
            {
                IsFoil = dto.IsFoil,
                InventoryCardStatusId = dto.InventoryCardStatusId,
                MultiverseId = dto.MultiverseId,
                VariantType = dto.VariantType,
                Name = dto.Name,
                Set = dto.Set,
                Id = dto.Id,
                DeckCards = MapInventoryDeckCardDtos(dto.DeckCards),
            };

            return result;
        }

        private static List<InventoryDeckCard> MapInventoryDeckCardDtos(List<InventoryDeckCardDto> cards)
        {
            if(cards == null)
            {
                return null;
            }

            List<InventoryDeckCard> result = cards.Select(x => new InventoryDeckCard()
            {
                DeckCardCategory = x.DeckCardCategory,
                DeckId = x.DeckId,
                DeckName = x.DeckName,
                Id = x.Id,
                InventoryCardId = x.InventoryCardId,
            }).ToList();

            return result;
        }

        #endregion

    }
}
