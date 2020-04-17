using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.UI.Legacy.Models;
using Carpentry.Data.Interfaces;

namespace Carpentry.UI.Legacy.Util
{
    public class MapperService : IMapperService
    {
        IDataReferenceService _refs;
        public MapperService(IDataReferenceService referenceService)
        {
            _refs = referenceService;
        }
        //AppFilters
        //DeckCard
        public DeckCard ToModel(DeckCardDto dto)
        {
            DeckCard deckCard = new DeckCard
            {
                InventoryCard = ToModel(dto.InventoryCard),
                CategoryId = dto.CategoryId,
                DeckId = dto.DeckId,
                Id = dto.Id,
            };
            return deckCard;
        }

        //DeckDetail
        public async Task<DeckDetailDto> ToDto(DeckDetail detail)
        {
            DeckDetailDto result = new DeckDetailDto()
            {
                CardDetails = ToDto(detail.CardDetails),
                CardOverviews = ToDto(detail.CardOverviews),
                Props = await ToDto(detail.Props),
                Stats = ToDto(detail.Stats),
            };
            return result;
        }

        //DeckProperties
        public async Task<DeckProperties> ToModel(DeckPropertiesDto props)
        {
            DeckProperties result = new DeckProperties()
            {
                BasicB = props.BasicB,
                BasicG = props.BasicG,
                BasicR = props.BasicR,
                BasicU = props.BasicU,
                BasicW = props.BasicW,
                FormatId = (await _refs.GetMagicFormat(props.Format)).Id,
                //props.Format,
                Id = props.Id,
                Name = props.Name,
                Notes = props.Notes,
            };
            return result;
        }
        public async Task<DeckPropertiesDto> ToDto(DeckProperties props)
        {
            var magicFormat = await _refs.GetMagicFormat(props.FormatId);

            DeckPropertiesDto result = new DeckPropertiesDto()
            {
                BasicB = props.BasicB,
                BasicG = props.BasicG,
                BasicR = props.BasicR,
                BasicU = props.BasicU,
                BasicW = props.BasicW,
                //Format = x.Format,
                Format = magicFormat.Name,
                Id = props.Id,
                Name = props.Name,
                Notes = props.Notes,
            };

            return result;
        }
        public async Task<List<DeckPropertiesDto>> ToDto(IEnumerable<DeckProperties> props)
        {
            var allFormats = await _refs.GetAllMagicFormats();

            List<DeckPropertiesDto> result = props.Select(x => new DeckPropertiesDto()
            {
                BasicB = x.BasicB,
                BasicG = x.BasicG,
                BasicR = x.BasicR,
                BasicU = x.BasicU,
                BasicW = x.BasicW,
                //Format = x.Format,
                Format = allFormats.Where(format => format.Id == x.FormatId).FirstOrDefault().Name,
                Id = x.Id,
                Name = x.Name,
                Notes = x.Notes,
            }).ToList();

            return result;
        }

        //DeckStats
        public DeckStatsDto ToDto(DeckStats dto)
        {
            DeckStatsDto result = new DeckStatsDto
            {
                ColorIdentity = dto.ColorIdentity,
                CostCounts = dto.CostCounts,
                TotalCost = dto.TotalCost,
                TotalCount = dto.TotalCount,
                TypeCounts = dto.TypeCounts,
            };
            return result;
        }

        //FilterOption
        public List<FilterOptionDto> ToDto(IEnumerable<FilterOption> filters)
        {
            List<FilterOptionDto> result = filters.Select(model => new FilterOptionDto
            {
                Name = model.Name,
                Value = model.Value,
            }).ToList();

            return result;
        }

        //InventoryCard
        public InventoryCard ToModel(InventoryCardDto card)
        {
            InventoryCard result = new InventoryCard
            {
                Id = card.Id,
                DeckCards = card.DeckCards.Select(x => ToModel(x)).ToList(),
                InventoryCardStatusId = card.InventoryCardStatusId,
                IsFoil = card.IsFoil,
                MultiverseId = card.MultiverseId,
                Name = card.Name,
                Set = card.Set,
                VariantType = card.VariantType,
            };
            return result;
        }
        public List<InventoryCard> ToModel(List<InventoryCardDto> cards)
        {
            List<InventoryCard> result = cards.Select(x => ToModel(x)).ToList();
            return result;
        }
        public InventoryCardDto ToDto(InventoryCard card)
        {
            InventoryCardDto result = new InventoryCardDto
            {
                Id = card.Id,
                DeckCards = card.DeckCards.Select(x => ToDto(x)).ToList(),
                InventoryCardStatusId = card.InventoryCardStatusId,
                IsFoil = card.IsFoil,
                MultiverseId = card.MultiverseId,
                Name = card.Name,
                Set = card.Set,
                VariantType = card.VariantType,
            };
            return result;
        }
        public List<InventoryCardDto> ToDto(List<InventoryCard> cards)
        {
            List<InventoryCardDto> result = cards.Select(x => ToDto(x)).ToList();
            return result;
        }

        //InventoryDeckCard
        public InventoryDeckCardDto ToDto(InventoryDeckCard invDeckCard)
        {
            InventoryDeckCardDto result = new InventoryDeckCardDto()
            {
                DeckCardCategory = invDeckCard.DeckCardCategory,
                DeckId = invDeckCard.DeckId,
                DeckName = invDeckCard.DeckName,
                Id = invDeckCard.Id,
                InventoryCardId = invDeckCard.InventoryCardId,
            };
            return result;
        }
        public InventoryDeckCard ToModel(InventoryDeckCardDto invDeckCard)
        {
            InventoryDeckCard result = new InventoryDeckCard()
            {
                DeckCardCategory = invDeckCard.DeckCardCategory,
                DeckId = invDeckCard.DeckId,
                DeckName = invDeckCard.DeckName,
                Id = invDeckCard.Id,
                InventoryCardId = invDeckCard.InventoryCardId,
            };
            return result;
        }

        //InventoryDetail
        public InventoryDetailDto ToDto(InventoryDetail inventoryDetail)
        {
            InventoryDetailDto result = new InventoryDetailDto()
            {
                Cards = inventoryDetail.Cards.Select(x => ToDto(x)).ToList(),
                InventoryCards = inventoryDetail.InventoryCards.Select(x => ToDto(x)).ToList(),
                Name = inventoryDetail.Name,
            };
            return result;
        }

        //InventoryOverview
        public List<InventoryOverviewDto> ToDto(IEnumerable<InventoryOverview> overviews)
        {
            List<InventoryOverviewDto> result = overviews.Select(x => new InventoryOverviewDto
            {
                Cmc = x.Cmc,
                Cost = x.Cost,
                Count = x.Count,
                Description = x.Description,
                Id = x.Id,
                Img = x.Img,
                Name = x.Name,
                Type = x.Type,
            }).ToList();

            return result;
        }

        //MagicCard
        public MagicCardDto ToDto(MagicCard card)
        {
            MagicCardDto result = new MagicCardDto
            {
                Name = card.Name,
                Cmc = card.Cmc,
                ColorIdentity = card.ColorIdentity,
                Colors = card.Colors,
                Legalities = card.Legalities,
                ManaCost = card.ManaCost,
                MultiverseId = card.MultiverseId,
                Prices = card.Prices,
                Rarity = card.Rarity,
                Set = card.Set,
                Text = card.Text,
                Type = card.Type,
                Variants = card.Variants,
            };

            return result;
        }
        public List<MagicCardDto> ToDto(IEnumerable<MagicCard> cardList)
        {
            List<MagicCardDto> result = cardList.Select(card => ToDto(card)).ToList();

            //List<MagicCardDto> result = cardList.Select(card => new MagicCardDto
            //{
            //    Name = card.Name,
            //    Cmc = card.Cmc,
            //    ColorIdentity = card.ColorIdentity,
            //    Colors = card.Colors,
            //    Legalities = card.Legalities,
            //    ManaCost = card.ManaCost,
            //    MultiverseId = card.MultiverseId,
            //    Prices = card.Prices,
            //    Rarity = card.Rarity,
            //    Set = card.Set,
            //    Text = card.Text,
            //    Type = card.Type,
            //    Variants = card.Variants,
            //}).ToList();

            return result;
        }
        public MagicCard ToModel(MagicCardDto card)
        {
            MagicCard result = new MagicCard
            {
                Name = card.Name,
                Cmc = card.Cmc,
                ColorIdentity = card.ColorIdentity,
                Colors = card.Colors,
                Legalities = card.Legalities,
                ManaCost = card.ManaCost,
                MultiverseId = card.MultiverseId,
                Prices = card.Prices,
                Rarity = card.Rarity,
                Set = card.Set,
                Text = card.Text,
                Type = card.Type,
                Variants = card.Variants,
            };
            return result;
        }

        #region Old mapper logic

        //public MagicCardDto(MagicCard model)
        //{
        //    Name = model.Name;
        //    Cmc = model.Cmc;
        //    ColorIdentity = model.ColorIdentity;
        //    Colors = model.Colors;
        //    Legalities = model.Legalities;
        //    ManaCost = model.ManaCost;
        //    MultiverseId = model.MultiverseId;
        //    Prices = model.Prices;
        //    Rarity = model.Rarity;
        //    Set = model.Set;
        //    Text = model.Text;
        //    Type = model.Type;
        //    Variants = model.Variants;
        //}

        //public MagicCard ToModel()
        //{
        //    MagicCard result = new MagicCard
        //    {
        //        Name = Name,
        //        Cmc = Cmc,
        //        ColorIdentity = ColorIdentity,
        //        Colors = Colors,
        //        Legalities = Legalities,
        //        ManaCost = ManaCost,
        //        MultiverseId = MultiverseId,
        //        Prices = Prices,
        //        Rarity = Rarity,
        //        Set = Set,
        //        Text = Text,
        //        Type = Type,
        //        Variants = Variants,
        //    };
        //    return result;
        //}

        //public InventoryOverviewDto(InventoryOverview model)
        //{
        //    Cmc = model.Cmc;
        //    Cost = model.Cost;
        //    Count = model.Count;
        //    Description = model.Description;
        //    Id = model.Id;
        //    Img = model.Img;
        //    Name = model.Name;
        //    Type = model.Type;
        //}

        //public InventoryOverview ToModel()
        //{
        //    InventoryOverview result = new InventoryOverview
        //    {
        //        Cmc = Cmc,
        //        Cost = Cost,
        //        Count = Count,
        //        Description = Description,
        //        Id = Id,
        //        Img = Img,
        //        Name = Name,
        //        Type = Type,
        //    };
        //    return result;
        //}

        //public InventoryDetailDto(InventoryDetail model)
        //{
        //    Cards = model.Cards.Select(x => new MagicCardDto(x)).ToList();
        //    InventoryCards = model.InventoryCards.Select(x => new InventoryCardDto(x)).ToList();
        //    Name = model.Name;
        //}

        //public InventoryDetail ToModel()
        //{
        //    InventoryDetail inventoryDetail = new InventoryDetail
        //    {
        //        Cards = Cards.Select(x => x.ToModel()).ToList(),
        //        InventoryCards = InventoryCards.Select(x => x.ToModel()).ToList(),
        //        Name = Name,
        //    };
        //    return inventoryDetail;
        //}

        //public InventoryDeckCardDto(InventoryDeckCard model)
        //{
        //    DeckCardCategory = model.DeckCardCategory;
        //    DeckId = model.DeckId;
        //    DeckName = model.DeckName;
        //    Id = model.Id;
        //    InventoryCardId = model.InventoryCardId;
        //}

        //public InventoryDeckCard ToModel()
        //{
        //    InventoryDeckCard result = new InventoryDeckCard
        //    {
        //        DeckCardCategory = DeckCardCategory,
        //        DeckId = DeckId,
        //        DeckName = DeckName,
        //        Id = Id,
        //        InventoryCardId = InventoryCardId,
        //    };
        //    return result;
        //}

        //public InventoryCardDto()
        //{
        //    DeckCards = new List<InventoryDeckCardDto>();
        //}

        //public InventoryCardDto(InventoryCard model)
        //{
        //    Id = model.Id;
        //    DeckCards = model.DeckCards.Select(x => new InventoryDeckCardDto(x)).ToList();
        //    InventoryCardStatusId = model.InventoryCardStatusId;
        //    IsFoil = model.IsFoil;
        //    MultiverseId = model.MultiverseId;
        //    Name = model.Name;
        //    Set = model.Set;
        //    VariantType = model.VariantType;
        //}

        //public InventoryCard ToModel()
        //{
        //    InventoryCard result = new InventoryCard
        //    {
        //        Id = Id,
        //        DeckCards = DeckCards.Select(x => x.ToModel()).ToList(),
        //        InventoryCardStatusId = InventoryCardStatusId,
        //        IsFoil = IsFoil,
        //        MultiverseId = MultiverseId,
        //        Name = Name,
        //        Set = Set,
        //        VariantType = VariantType,
        //    };
        //    return result;
        //}

        //public DeckStatsDto(DeckStats model)
        //{
        //    ColorIdentity = model.ColorIdentity;
        //    CostCounts = model.CostCounts;
        //    TotalCost = model.TotalCost;
        //    TotalCount = model.TotalCount;
        //    TypeCounts = model.TypeCounts;
        //}

        //public DeckStats ToModel()
        //{
        //    DeckStats deckStats = new DeckStats
        //    {
        //        ColorIdentity = ColorIdentity,
        //        CostCounts = CostCounts,
        //        TotalCost = TotalCost,
        //        TotalCount = TotalCount,
        //        TypeCounts = TypeCounts,
        //    };
        //    return deckStats;
        //}

        //public FilterOptionDto(FilterOption model)
        //{
        //    Name = model.Name;
        //    Value = model.Value;
        //}

        //public DeckPropertiesDto()
        //{

        //}

        //public DeckPropertiesDto(DeckProperties props)
        //{
        //    BasicB = props.BasicB;
        //    BasicG = props.BasicG;
        //    BasicR = props.BasicR;
        //    BasicU = props.BasicU;
        //    BasicW = props.BasicW;
        //    Format = props.Format;
        //    Id = props.Id;
        //    Name = props.Name;
        //    Notes = props.Notes;
        //}



        //public DeckDetailDto(DeckDetail model)
        //{
        //    CardDetails = model.CardDetails.Select(x => new InventoryCardDto(x)).ToList();
        //    CardOverviews = model.CardOverviews.Select(x => new InventoryOverviewDto(x)).ToList();
        //    Props = new DeckPropertiesDto(model.Props);
        //    Stats = new DeckStatsDto(model.Stats);
        //}

        //public DeckDetail ToModel()
        //{
        //    DeckDetail result = new DeckDetail
        //    {
        //        CardDetails = CardDetails.Select(x => x.ToModel()).ToList(),
        //        CardOverviews = CardOverviews.Select(x => x.ToModel()).ToList(),
        //        Props = Props.ToModel(),
        //        Stats = Stats.ToModel(),
        //    };
        //    return result;
        //}


        //public DeckCardDto()
        //{
        //}

        //public DeckCardDto(DeckCard data)
        //{
        //    Id = data.Id;
        //    DeckId = data.DeckId;
        //    CategoryId = data.CategoryId;
        //    InventoryCard = new InventoryCardDto(data.InventoryCard);
        //}


















        #endregion
    }
}
