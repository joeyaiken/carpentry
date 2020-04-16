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

        //data reference service?
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


        private static IEnumerable<MagicCard> MapInventoryQueryToMagicCardObject(IEnumerable<Data.DataModels.CardData> query)
        {
            IEnumerable<MagicCard> result = query.Select(card => new MagicCard()
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
            });
            return result;
        }

        #endregion

        #region Inventory related methods

        public async Task<int> AddInventoryCard(InventoryCard dto)
        {
            await _dataUpdateService.EnsureCardDefinitionExists(dto.MultiverseId);

            Data.DataModels.CardVariantTypeData cardVariant = await _referenceService.GetCardVariantTypeByName(dto.VariantType);

            var newInventoryCard = new Data.DataModels.InventoryCardData()
            {
                IsFoil = dto.IsFoil,
                InventoryCardStatusId = dto.InventoryCardStatusId,
                MultiverseId = dto.MultiverseId,
                VariantType = cardVariant,
            };

            newInventoryCard.Id = await _inventoryRepo.AddInventoryCard(newInventoryCard);

            return newInventoryCard.Id;

            //return newId;
        }

        public async Task AddInventoryCardBatch(IEnumerable<InventoryCard> cards)
        {

            //Ensure all cards exist in the repo


            var distinctIDs = cards.Select(x => x.MultiverseId).Distinct().ToList();

            for(int i = 0; i < distinctIDs.Count(); i++)
            {
                await _dataUpdateService.EnsureCardDefinitionExists(distinctIDs[i]);
            }

            var newCardsTasks = cards.Select(async x => new Data.DataModels.InventoryCardData()
            {
                IsFoil = x.IsFoil,
                InventoryCardStatusId = x.InventoryCardStatusId,
                MultiverseId = x.MultiverseId,
                //VariantType = _cardContext.VariantTypes.FirstOrDefault(v => v.Name == x.VariantType),
                VariantType = await _referenceService.GetCardVariantTypeByName(x.VariantType),

            }).ToList();

            var newCards = (await Task.WhenAll(newCardsTasks)).ToList();

            await _inventoryRepo.AddInventoryCardBatch(newCards);
        }

        public async Task UpdateInventoryCard(InventoryCard dto)
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
            dbCard.InventoryCardStatusId = dto.InventoryCardStatusId;

            await _inventoryRepo.UpdateInventoryCard(dbCard);
        }

        public async Task DeleteInventoryCard(int id)
        {
            await _inventoryRepo.DeleteInventoryCard(id);
        }


        private static InventoryOverview MapCardResultToInventoryOverview(CardOverviewResult data)
        {
            InventoryOverview result = new InventoryOverview()
            {
                Cmc = data.Cmc,
                Cost = data.Cost,
                Count = data.Count,
                //Description = data.,
                Id = data.Id,
                Img = data.Img,
                Name = data.Name,
                Type = data.Type,
            };
            return result;
        }

        public async Task<IEnumerable<InventoryOverview>> GetInventoryOverviews(InventoryQueryParameter param)
        {
            if(param == null)
            {
                throw new ArgumentNullException("param");
            }

            IEnumerable<CardOverviewResult> result = await _queryService.GetInventoryOverviews(param);

            IEnumerable<InventoryOverview> mappedResult = result.Select(x => MapCardResultToInventoryOverview(x));

            return mappedResult;
        }

        public async Task<InventoryDetail> GetInventoryDetailByName(string name)
        {
            InventoryDetail result = new InventoryDetail()
            {
                Name = name,
                Cards = new List<MagicCard>(),
                InventoryCards = new List<InventoryCard>(),
            };

            //inv cards

            //GetInventoryCardsByName -> InventoryCardResult

            List<InventoryCard> inventoryCards = (await _queryService.GetInventoryCardsByName(name))
                .Select(x => new InventoryCard()
                {
                    Id = x.Id,
                    IsFoil = x.IsFoil,
                    InventoryCardStatusId = x.InventoryCardStatusId,
                    MultiverseId = x.MultiverseId,
                    VariantType = x.VariantType,
                    Name = x.Name,
                    Set = x.Set,
                    DeckCards = x.DeckCards.Select(c => new InventoryDeckCard
                    {
                        Id = c.Id,
                        DeckId = c.DeckId,
                        InventoryCardId = c.InventoryCardId,
                        DeckName = c.DeckName,
                    }).ToList()
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
            var cardDefinitions = await _cardDataRepo.GetCardsByName(name);

            result.Cards = MapInventoryQueryToMagicCardObject(cardDefinitions).ToList();

            return result;
        }

        #endregion

    }
}
