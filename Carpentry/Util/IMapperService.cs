using Carpentry.Logic.Models;
using Carpentry.UI.Legacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Legacy.Util
{
    public interface IMapperService
    {

        //AppFilters
        //DeckCard
        DeckCard ToModel(LegacyDeckCardDto dto);

        //DeckDetail
        Task<LegacyDeckDetailDto> ToDto(DeckDetail detail);

        //DeckProperties
        Task<DeckProperties> ToModel(LegacyDeckPropertiesDto props);
        Task<LegacyDeckPropertiesDto> ToDto(DeckProperties props);
        Task<List<LegacyDeckPropertiesDto>> ToDto(IEnumerable<DeckProperties> props);

        //DeckStats
        LegacyDeckStatsDto ToDto(DeckStats dto);

        //FilterOption
        List<LegacyFilterOptionDto> ToDto(IEnumerable<FilterOption> filters);

        //InventoryCard
        InventoryCard ToModel(LegacyInventoryCardDto card);
        List<InventoryCard> ToModel(List<LegacyInventoryCardDto> cards);
        LegacyInventoryCardDto ToDto(InventoryCard card);
        List<LegacyInventoryCardDto> ToDto(List<InventoryCard> cards);

        //InventoryDeckCard
        LegacyInventoryDeckCardDto ToDto(InventoryDeckCard invDeckCard);
        InventoryDeckCard ToModel(LegacyInventoryDeckCardDto invDeckCard);

        //InventoryDetail
        LegacyInventoryDetailDto ToDto(InventoryDetail inventoryDetail);

        //InventoryOverview
        List<LegacyInventoryOverviewDto> ToDto(IEnumerable<InventoryOverview> overviews);

        //MagicCard
        LegacyMagicCardDto ToDto(MagicCard card);
        List<LegacyMagicCardDto> ToDto(IEnumerable<MagicCard> cardList);
        MagicCard ToModel(LegacyMagicCardDto card);














        ////AppFilters
        ////DeckCard
        //DeckCard ToModel(DeckCardDto dto);
        ////DeckDetail
        //DeckDetailDto ToDto(DeckDetail detail);
        ////DeckProperties
        //Task<DeckProperties> ToModel(DeckPropertiesDto props);
        //List<DeckPropertiesDto> ToDto(IEnumerable<DeckProperties> props);
        ////DeckStats
        ////FilterOption
        //List<FilterOptionDto> ToDto(IEnumerable<FilterOption> filters);
        ////InventoryCard
        //InventoryCard ToModel(InventoryCardDto card);
        //List<InventoryCard> ToModel(List<InventoryCardDto> cards);
        ////InventoryDeckCard
        ////InventoryDetail
        //InventoryDetailDto ToDto(InventoryDetail inventoryDetail);
        ////InventoryOverview
        //List<InventoryOverviewDto> ToDto(IEnumerable<InventoryOverview> overviews);
        ////MagicCard
        //List<MagicCardDto> ToDto(IEnumerable<MagicCard> cardList);
    }
}
