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
        DeckCard ToModel(DeckCardDto dto);

        //DeckDetail
        Task<DeckDetailDto> ToDto(DeckDetail detail);

        //DeckProperties
        Task<DeckProperties> ToModel(DeckPropertiesDto props);
        Task<DeckPropertiesDto> ToDto(DeckProperties props);
        Task<List<DeckPropertiesDto>> ToDto(IEnumerable<DeckProperties> props);

        //DeckStats
        DeckStatsDto ToDto(DeckStats dto);

        //FilterOption
        List<FilterOptionDto> ToDto(IEnumerable<FilterOption> filters);

        //InventoryCard
        InventoryCard ToModel(InventoryCardDto card);
        List<InventoryCard> ToModel(List<InventoryCardDto> cards);
        InventoryCardDto ToDto(InventoryCard card);
        List<InventoryCardDto> ToDto(List<InventoryCard> cards);

        //InventoryDeckCard
        InventoryDeckCardDto ToDto(InventoryDeckCard invDeckCard);
        InventoryDeckCard ToModel(InventoryDeckCardDto invDeckCard);

        //InventoryDetail
        InventoryDetailDto ToDto(InventoryDetail inventoryDetail);

        //InventoryOverview
        List<InventoryOverviewDto> ToDto(IEnumerable<InventoryOverview> overviews);

        //MagicCard
        MagicCardDto ToDto(MagicCard card);
        List<MagicCardDto> ToDto(IEnumerable<MagicCard> cardList);
        MagicCard ToModel(MagicCardDto card);














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
