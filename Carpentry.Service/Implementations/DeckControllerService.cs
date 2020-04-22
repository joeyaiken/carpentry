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
        //public async Task<int> AddDeck(DeckPropertiesDto props)
        //{
        //    DataReferenceValue<int> deckFormat = await _referenceService.GetMagicFormat(props.Format);

        //    DeckProperties deckModel = MapDeckPropertiesDto(props, deckFormat.Id);

        //    int newId = await _deckService.AddDeck(deckModel);

        //    return newId;
        //}

        //public async Task UpdateDeck(DeckPropertiesDto props)
        //{
        //    DataReferenceValue<int> deckFormat = await _referenceService.GetMagicFormat(props.Format);

        //    DeckProperties deckModel = MapDeckPropertiesDto(props, deckFormat.Id);

        //    await _deckService.UpdateDeck(deckModel);
        //}

        //public async Task DeleteDeck(int deckId)
        //{
        //    await _deckService.DeleteDeck(deckId);
        //}

        //public async Task<IEnumerable<DeckPropertiesDto>> GetDeckOverviews()
        //{
        //    var serviceResult = await _deckService.GetDeckOverviews();

        //    var formats = await _referenceService.GetAllMagicFormats();

        //    List<DeckPropertiesDto> mappedResults = serviceResult
        //        .Select(props => MapDeckPropertiesDto(props, formats.FirstOrDefault(f => f.Id == props.FormatId).Name))
        //        .ToList();

        //    return mappedResults;
        //}

        //public async Task<DeckDetailDto> GetDeckDetail(int deckId)
        //{
        //    var serviceResult = await _deckService.GetDeckDetail(deckId);

        //    var deckFormat = await _referenceService.GetMagicFormat(serviceResult.Props.FormatId);

        //    DeckDetailDto mappedResult = MapDeckDetailDto(serviceResult, deckFormat.Name);

        //    return mappedResult;
        //}

        //public async Task AddDeckCard(DeckCardDto dto)
        //{
        //    DeckCard mappedCard = MapDeckCardDto(dto);

        //    await _deckService.AddDeckCard(mappedCard);
        //}

        //public async Task AddDeckCardBatch(IEnumerable<DeckCardDto> dto)
        //{
        //    List<DeckCard> mappedCards = dto.Select(x => MapDeckCardDto(x)).ToList();

        //    await _deckService.AddDeckCardBatch(mappedCards);
        //}

        //public async Task UpdateDeckCard(DeckCardDto dto)
        //{
        //    DeckCard mappedCard = MapDeckCardDto(dto);

        //    await _deckService.UpdateDeckCard(mappedCard);
        //}

        //public async Task DeleteDeckCard(int deckCardId)
        //{
        //    await _deckService.DeleteDeckCard(deckCardId);
        //}

        #region private

        //private static DeckCard MapDeckCardDto(DeckCardDto dto)
        //{
        //    if (dto == null)
        //    {
        //        return null;
        //    }

        //    DeckCard result = new DeckCard()
        //    {
        //        CategoryId = dto.CategoryId,
        //        DeckId = dto.DeckId,
        //        InventoryCard = MapInventoryCardDto(dto.InventoryCard),
        //    };

        //    return result;
        //}

        //private static DeckCardDto MapDeckCardDto(DeckCard dto)
        //{
        //    if (dto == null)
        //    {
        //        return null;
        //    }

        //    DeckCardDto result = new DeckCardDto()
        //    {
        //        CategoryId = dto.CategoryId,
        //        DeckId = dto.DeckId,
        //        InventoryCard = MapInventoryCardDto(dto.InventoryCard),
        //    };

        //    return result;
        //}

        //private static DeckDetailDto MapDeckDetailDto(DeckDetail detail, string formatName)
        //{


        //    DeckDetailDto result = new DeckDetailDto()
        //    {
        //        CardDetails = detail.CardDetails.Select(x => MapInventoryCardDto(x)).ToList(),
        //        CardOverviews = MapInventoryOverviewDtos(detail.CardOverviews),
        //        Props = MapDeckPropertiesDto(detail.Props, formatName),
        //        Stats = MapDeckStatsDto(detail.Stats),
        //    };

        //    return result;
        //}

        //private static DeckProperties MapDeckPropertiesDto(DeckPropertiesDto props, int formatId)
        //{
        //    DeckProperties deckModel = new DeckProperties()
        //    {
        //        Id = props.Id,
        //        Name = props.Name,
        //        FormatId = formatId,
        //        Notes = props.Notes,

        //        BasicW = props.BasicW,
        //        BasicU = props.BasicU,
        //        BasicB = props.BasicB,
        //        BasicR = props.BasicR,
        //        BasicG = props.BasicG,
        //    };
        //    return deckModel;
        //}

        //private static DeckPropertiesDto MapDeckPropertiesDto(DeckProperties props, string formatName)
        //{
        //    DeckPropertiesDto deckModel = new DeckPropertiesDto()
        //    {
        //        Id = props.Id,
        //        Name = props.Name,
        //        Format = formatName,
        //        Notes = props.Notes,

        //        BasicW = props.BasicW,
        //        BasicU = props.BasicU,
        //        BasicB = props.BasicB,
        //        BasicR = props.BasicR,
        //        BasicG = props.BasicG,
        //    };
        //    return deckModel;
        //}

        //private static DeckStatsDto MapDeckStatsDto(DeckStats dto)
        //{
        //    DeckStatsDto result = new DeckStatsDto
        //    {
        //        ColorIdentity = dto.ColorIdentity,
        //        CostCounts = dto.CostCounts,
        //        TotalCost = dto.TotalCost,
        //        TotalCount = dto.TotalCount,
        //        TypeCounts = dto.TypeCounts,
        //    };
        //    return result;
        //}

        //private static InventoryCard MapInventoryCardDto(InventoryCardDto dto)
        //{
        //    if(dto == null)
        //    {
        //        return null;
        //    }

        //    InventoryCard result = new InventoryCard()
        //    {
        //        IsFoil = dto.IsFoil,
        //        InventoryCardStatusId = dto.InventoryCardStatusId,
        //        MultiverseId = dto.MultiverseId,
        //        VariantType = dto.VariantType,
        //        Name = dto.Name,
        //        Set = dto.Set,
        //        Id = dto.Id,
        //        DeckCards = MapInventoryDeckCardDtos(dto.DeckCards),
        //    };

        //    return result;
        //}

        //private static InventoryCardDto MapInventoryCardDto(InventoryCard card)
        //{
        //    InventoryCardDto result = new InventoryCardDto
        //    {
        //        Id = card.Id,
        //        DeckCards = card.DeckCards.Select(x => MapInventoryDeckCardDto(x)).ToList(),
        //        InventoryCardStatusId = card.InventoryCardStatusId,
        //        IsFoil = card.IsFoil,
        //        MultiverseId = card.MultiverseId,
        //        Name = card.Name,
        //        Set = card.Set,
        //        VariantType = card.VariantType,
        //    };
        //    return result;
        //}

        //private static List<InventoryDeckCard> MapInventoryDeckCardDtos(List<InventoryDeckCardDto> cards)
        //{
        //    if(cards == null)
        //    {
        //        return null;
        //    }

        //    List<InventoryDeckCard> result = cards.Select(x => new InventoryDeckCard()
        //    {
        //        DeckCardCategory = x.DeckCardCategory,
        //        DeckId = x.DeckId,
        //        DeckName = x.DeckName,
        //        Id = x.Id,
        //        InventoryCardId = x.InventoryCardId,
        //    }).ToList();

        //    return result;
        //}

        //private static List<InventoryOverviewDto> MapInventoryOverviewDtos(IEnumerable<InventoryOverview> overviews)
        //{
        //    List<InventoryOverviewDto> result = overviews.Select(x => new InventoryOverviewDto
        //    {
        //        Cmc = x.Cmc,
        //        Cost = x.Cost,
        //        Count = x.Count,
        //        Description = x.Description,
        //        Id = x.Id,
        //        Img = x.Img,
        //        Name = x.Name,
        //        Type = x.Type,
        //    }).ToList();

        //    return result;
        //}

        //private static InventoryDeckCardDto MapInventoryDeckCardDto(InventoryDeckCard invDeckCard)
        //{
        //    InventoryDeckCardDto result = new InventoryDeckCardDto()
        //    {
        //        DeckCardCategory = invDeckCard.DeckCardCategory,
        //        DeckId = invDeckCard.DeckId,
        //        DeckName = invDeckCard.DeckName,
        //        Id = invDeckCard.Id,
        //        InventoryCardId = invDeckCard.InventoryCardId,
        //    };
        //    return result;
        //}

        #endregion

    }
}
