using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Carpentry.Data.QueryResults;
//using Carpentry.Data.DataContext;

namespace Carpentry.Logic.Implementations
{
    public class InventoryService : IInventoryService
    {

        private readonly IInventoryDataRepo _inventoryRepo;

        private readonly IDataUpdateService _dataUpdateService;

        private readonly IDataQueryService _queryService;

        private readonly IDataReferenceService _referenceService;

        private readonly ICardDataRepo _cardDataRepo;

        public InventoryService(
            IInventoryDataRepo inventoryRepo,
            IDataUpdateService dataUpdateService,
            IDataQueryService queryService,
            IDataReferenceService referenceService,
            ICardDataRepo cardDataRepo
        )
        {
            _inventoryRepo = inventoryRepo;
            _dataUpdateService = dataUpdateService;
            _queryService = queryService;
            _referenceService = referenceService;
            _cardDataRepo = cardDataRepo;
        }

        #region private methods


        private static InventoryOverviewDto MapCardResultToInventoryOverview(CardOverviewResult data)
        {
            InventoryOverviewDto result = new InventoryOverviewDto()
            {
                Cmc = data.Cmc,
                Cost = data.Cost,
                Count = data.Count,
                //Description = data.,
                Id = data.Id,
                Img = data.Img,
                Name = data.Name,
                Type = data.Type,
                Price = data.Price,
                IsFoil = data.IsFoil,
                Category = data.Category,
                Description = data.Category,
                Variant = data.Variant,
            };
            return result;
        }

        private static List<MagicCardDto> MapInventoryQueryToMagicCardObject(List<Data.DataModels.CardData> query)
        {
            List<MagicCardDto> result = query.Select(card => new MagicCardDto()
            {
                Cmc = card.Cmc,
                ManaCost = card.ManaCost,
                MultiverseId = card.Id,
                Name = card.Name,

                //Prices = card.Variants.ToDictionary(v => (v.)  )

                Prices = card.Variants.SelectMany(x => new[]
                {
                            new {
                                Name = x.Type.Name,
                                Price = x.Price,
                            },
                            new {
                                Name = $"{x.Type.Name}_foil",
                                Price = x.PriceFoil,
                            }
                        }).ToDictionary(v => v.Name, v => v.Price),

                //Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
                Variants = card.Variants.Select(v => new { v.Type.Name, v.ImageUrl }).ToDictionary(v => v.Name, v => v.ImageUrl),
                Colors = card.CardColors.Select(c => c.ManaType.Name).ToList(),
                Rarity = card.Rarity.Name,
                Set = card.Set.Code,
                Text = card.Text,
                Type = card.Type,
                ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList(),
                Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
            }).ToList();
            return result;
        }

        #endregion

        #region Inventory Card add/update/delete

        public async Task<int> AddInventoryCard(InventoryCardDto dto)
        {
            await _dataUpdateService.EnsureCardDefinitionExists(dto.MultiverseId);

            DataReferenceValue<int> cardVariant = await _referenceService.GetCardVariantTypeByName(dto.VariantName);

            var newInventoryCard = new Data.DataModels.InventoryCardData()
            {
                IsFoil = dto.IsFoil,
                InventoryCardStatusId = dto.StatusId,
                MultiverseId = dto.MultiverseId,
                VariantTypeId = cardVariant.Id,
            };

            newInventoryCard.Id = await _inventoryRepo.AddInventoryCard(newInventoryCard);

            return newInventoryCard.Id;
        }

        public async Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards)
        {

            //Ensure all cards exist in the repo


            var distinctIDs = cards.Select(x => x.MultiverseId).Distinct().ToList();

            for (int i = 0; i < distinctIDs.Count(); i++)
            {
                await _dataUpdateService.EnsureCardDefinitionExists(distinctIDs[i]);
            }

            var variantTypes = await _referenceService.GetAllCardVariantTypes();

            var newCards = cards.Select(x => new Data.DataModels.InventoryCardData()
            {
                IsFoil = x.IsFoil,
                InventoryCardStatusId = x.StatusId,
                MultiverseId = x.MultiverseId,
                VariantTypeId = variantTypes.FirstOrDefault(v => v.Name == x.VariantName).Id,

            }).ToList();

            await _inventoryRepo.AddInventoryCardBatch(newCards);
        }

        public async Task UpdateInventoryCard(InventoryCardDto dto)
        {
            //This probably should just:
            //  Take DTO from the UI layer
            //  Map the DTO to a DB model
            //  Send that DB model to the DB

            //I don't know why I'd need
            //  UI specific CS models/DTOs
            //  DB specific models / classes
            //  COMPLETELY SEPARATE models that are used to magically hold things when mapping from UI and DB
            //      UI <-> LOGIC <-> DATA
            //      More mapping == more chances for errors

            //Whatever, the Logic layer doesn't care what the UI layer is doing
            //It still needs to just map to something the DB can consume, the DB doesn't need a unique layer of mappings

            Carpentry.Data.DataModels.InventoryCardData dbCard = await _inventoryRepo.GetInventoryCardById(dto.Id);

            //currently only expecting to change the status with this method
            dbCard.InventoryCardStatusId = dto.StatusId;

            await _inventoryRepo.UpdateInventoryCard(dbCard);
        }

        public async Task UpdateInventoryCardBatch(IEnumerable<InventoryCardDto> batch)
        {
            foreach(var card in batch)
            {
                await UpdateInventoryCard(card);
            }
        }

        public async Task DeleteInventoryCard(int id)
        {
            await _inventoryRepo.DeleteInventoryCard(id);
        }

        public async Task DeleteInventoryCardBatch(IEnumerable<int> batch)
        {
            foreach(int id in batch)
            {
                await DeleteInventoryCard(id);
            }
        }

        #endregion Inventory Card add/update/delete

        #region Search

        public async Task<List<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            IEnumerable<CardOverviewResult> result = await _queryService.GetInventoryOverviews(param);

            List<InventoryOverviewDto> mappedResult = result.Select(x => MapCardResultToInventoryOverview(x)).ToList();

            return mappedResult;
        }

        public async Task<InventoryDetailDto> GetInventoryDetail(int cardId)
        {

            var matchingCard = await _cardDataRepo.GetCardData(cardId);

            if (matchingCard == null)
            {
                throw new Exception($"No card found for ID {cardId}");
            }


            InventoryDetailDto result = new InventoryDetailDto()
            {
                Name = matchingCard.Name,
                Cards = new List<MagicCardDto>(),
                InventoryCards = new List<InventoryCardDto>(),
            };

            //inv cards

            //GetInventoryCardsByName -> InventoryCardResult

            List<InventoryCardDto> inventoryCards = (await _queryService.GetInventoryCardsByName(matchingCard.Name))
                .Select(x => new InventoryCardDto()
                {
                    Id = x.Id,
                    IsFoil = x.IsFoil,
                    StatusId = x.InventoryCardStatusId,
                    MultiverseId = x.MultiverseId,
                    VariantName = x.VariantType,
                    Name = x.Name,
                    Set = x.Set,
                    //DeckCards = x.DeckCards.Select(c => new InventoryDeckCardDto
                    //{
                    //    Id = c.Id,
                    //    DeckId = c.DeckId,
                    //    InventoryCardId = c.InventoryCardId,
                    //    DeckName = c.DeckName,
                    //}).ToList()
                })
                .OrderBy(x => x.Id).ToList();

            //var inventoryCardsQuery = _inventoryRepo.QueryCardDefinitions().Where(x => x.Name == name)
            //    .SelectMany(x => x.InventoryCards)
            //    .Select(x => new InventoryCard()
            //    {
            //        Id = x.Id,
            //        IsFoil = x.IsFoil,
            //        InventoryCardStatusId = x.InventoryCardStatusId,
            //        MultiverseId = x.MultiverseId,
            //        VariantType = x.VariantType.Name,
            //        Name = x.Card.Name,
            //        Set = x.Card.Set.Code,
            //        DeckCards = x.DeckCards.Select(c => new InventoryDeckCard
            //        {
            //            Id = c.Id,
            //            DeckId = c.DeckId,
            //            InventoryCardId = c.InventoryCardId,
            //            DeckName = c.Deck.Name,
            //        }).ToList()
            //    })
            //    .OrderBy(x => x.Id);

            result.InventoryCards = inventoryCards;

            //card definitions
            //GetCardsByName | GetCardDefinitionsByName | GetCardDataByName -> CardData
            //Should this be from the query service or cardDataRepo?
            //var cardDefinitionsQuery = _inventoryRepo.QueryCardDefinitions().Where(x => x.Name == name);
            var cardDefinitions = await _cardDataRepo.GetCardsByName(matchingCard.Name);

            result.Cards = MapInventoryQueryToMagicCardObject(cardDefinitions).ToList();

            return result;
        }

        #endregion Search

        #region Collection Builder

        public async Task<List<InventoryOverviewDto>> GetCollectionBuilderSuggestions()
        {
            throw new NotImplementedException();
        }
        public async Task HideCollectionBuilderSuggestion(InventoryOverviewDto dto)
        {
            throw new NotImplementedException();
        }

        #endregion Collection Builder

        #region Trimming Tips

        public async Task<List<InventoryOverviewDto>> GetTrimmingTips()
        {
            throw new NotImplementedException();
        }
        public async Task HideTrimmingTip(InventoryOverviewDto dto)
        {
            throw new NotImplementedException();
        }

        #endregion Trimming Tips

        #region Import/Export

        public async Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto cardImportDto)
        {
            throw new NotImplementedException();
        }
        public async Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto dto)
        {
            throw new NotImplementedException();
        }
        public async Task<byte[]> ExportInventoryBackup()
        {
            throw new NotImplementedException();
        }

        #endregion Import/Export
    }
}
