using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.Interfaces;
using Carpentry.Data.DataModels;
using System.Linq;
using Carpentry.Data.QueryResults;
using Carpentry.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Carpentry.Data.DataModels.QueryResults;

namespace Carpentry.Logic.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryDataRepo _inventoryRepo;
        private readonly ICardDataRepo _cardDataRepo;
        private readonly CarpentryDataContext _cardContext;

        public InventoryService(
            IInventoryDataRepo inventoryRepo,
            ICardDataRepo cardDataRepo
            //fuck it, adding DB context directly because 
            ,CarpentryDataContext cardContext
        )
        {
            _inventoryRepo = inventoryRepo;
            _cardDataRepo = cardDataRepo;
            //repos are making querying too challenging
            _cardContext = cardContext;
        }

        public async Task<int> AddInventoryCard(InventoryCardDto dto)
        {
            //TODO - validate card ID, instead of blindly trusting it if > 0
            var newInventoryCard = new Data.DataModels.InventoryCardData()
            {
                IsFoil = dto.IsFoil,
                InventoryCardStatusId = dto.StatusId,
                CardId = dto.CardId,
            };

            //Does the DTO contain a valid CardId?
            if(newInventoryCard.CardId == 0)
            {
                //var matchingSet = await _cardDataRepo.GetCardSetByCode(dto.Set);

                //if(matchingSet == null || !matchingSet.IsTracked)
                //{
                //    throw new Exception("Cannot add inventory card, not a valid tracked set");
                //}

                var cardData = await _cardDataRepo.GetCardData(dto.Set, dto.CollectorNumber);

                //if (string.IsNullOrEmpty(dto.Set) || dto.CollectorNumber == 0)
                //{
                //    throw new Exception("Cannot add inventory card, not enough information to find a card");
                //}

                //If not, does the DTO contain a valid Set/CollectorNumber combo?
                //var cardData = await _cardDataRepo.GetCardData(dto.Set, dto.CollectorNumber);


                newInventoryCard.CardId = cardData.CardId;


            }



            //Is the set actually tracked?

            //Then, build a Data object and send to DB



            //await _dataUpdateService.EnsureCardDefinitionExists(dto.MultiverseId);
            //DataReferenceValue<int> cardVariant = await _coreDataRepo.GetCardVariantTypeByName(dto.VariantName);




            newInventoryCard.InventoryCardId = await _inventoryRepo.AddInventoryCard(newInventoryCard);

            return newInventoryCard.InventoryCardId;
        }
        //TODO - Use some smaller DTO that only has the 3 fields actually used
        public async Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards)
        {

            //TODO - Ensure all cards exist in the repo


            //var distinctIDs = cards.Select(x => x.MultiverseId).Distinct().ToList();

            //for (int i = 0; i < distinctIDs.Count(); i++)
            //{
            //    //await _dataUpdateService.EnsureCardDefinitionExists(distinctIDs[i]);
            //}

            //var variantTypes = await _coreDataRepo.GetAllCardVariantTypes();

            var newCards = cards.Select(x => new Data.DataModels.InventoryCardData()
            {
                IsFoil = x.IsFoil,
                InventoryCardStatusId = x.StatusId,
                CardId = x.CardId,
                //MultiverseId = x.MultiverseId,
                //VariantTypeId = variantTypes.FirstOrDefault(v => v.Name == x.VariantName).Id,

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

            Carpentry.Data.DataModels.InventoryCardData dbCard = await _inventoryRepo.GetInventoryCard(dto.Id);

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

        public async Task<InventoryDetailDto> GetInventoryDetail(int cardId)
        {

            var matchingCard = await _cardDataRepo.GetCardData(cardId);

            if (matchingCard == null)
            {
                throw new Exception($"No card found for ID {cardId}");
            }


            InventoryDetailDto result = new InventoryDetailDto()
            {
                CardId = cardId,
                Name = matchingCard.Name,
                Cards = new List<MagicCardDto>(),
                InventoryCards = new List<InventoryCardDto>(),
            };

            //inv cards

            //GetInventoryCardsByName -> InventoryCardResult

            List<InventoryCardDto> inventoryCards = (await _inventoryRepo.GetInventoryCardsByName(matchingCard.Name))
                .Select(x => new InventoryCardDto()
                {
                    Id = x.Id,
                    IsFoil = x.IsFoil,
                    StatusId = x.InventoryCardStatusId,
                    //MultiverseId = x.MultiverseId,
                    //VariantName = x.VariantType,
                    CardId = x.CardId,
                    CollectorNumber = x.CollectorNumber,

                    Name = x.Name,
                    Set = x.Set,

                    DeckId = x.DeckId,
                    DeckName = x.DeckName,
                    DeckCardId = x.DeckCardId,
                    DeckCardCategory = x.DeckCardCategory,

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

            result.Cards = cardDefinitions.Select(card => new MagicCardDto()
            {
                CardId = card.CardId,
                Cmc = card.Cmc,
                ManaCost = card.ManaCost,
                MultiverseId = card.MultiverseId,
                Name = card.Name,
                CollectionNumber = card.CollectorNumber,
                ImageUrl = card.ImageUrl,
                Price = card.Price,
                PriceFoil = card.PriceFoil,
                PriceTix = card.TixPrice,
                Colors = card.Color?.Split().ToList(),
                Rarity = card.Rarity.Name,
                Set = card.Set.Code,
                Text = card.Text,
                Type = card.Type,
                ColorIdentity = card.ColorIdentity.Split().ToList(),
                Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
            }).ToList();

            return result;
        }


        //Consider moving this region to a unique service
        #region Trimming Tool

        public async Task<List<TrimmingToolResult>> GetTrimmingToolCards(string setCode, int minCount, string filterBy, string searchGroup = null)
        {
            //need:
            //  inventory cards by print
            //  joined with inventoryCardsByName
            //  filtered accordingly

            var query = from uniqueCard in _cardContext.InventoryCardByUnique
                        join namedCard in _cardContext.InventoryCardByName
                        on uniqueCard.Name equals namedCard.Name
                        where uniqueCard.SetCode == setCode
                        //&& uniqueCard.TotalCount >= minCount
                        //&& (uniqueCard.InventoryCount + uniqueCard.DeckCount) >= minCount
                        select new { ByUnique = uniqueCard, ByName = namedCard };

            if (!string.IsNullOrEmpty(searchGroup))
            {
                switch (searchGroup)
                {
                    case "Red":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "R" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Blue":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "U" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Green":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "G" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "White":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "W" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Black":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "B" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Multicolored":
                        query = query.Where(x => x.ByUnique.ColorIdentity.Length > 1 && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Colorless":
                        query = query.Where(x => x.ByUnique.ColorIdentity.Length == 0 && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Lands":
                        query = query.Where(x => x.ByUnique.Type.Contains("Land") && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));// && !x.Type.Contains()
                        break;
                    case "RareMythic":
                        query = query.Where(x => x.ByUnique.RarityId == 'R' || x.ByUnique.RarityId == 'M');
                        break;
                }
            }

            switch (filterBy)
            {
                case "inventory":
                    query = query.Where(c => c.ByUnique.InventoryCount >= minCount);
                    break;

                case "owned":
                    query = query.Where(c => (c.ByUnique.InventoryCount + c.ByUnique.DeckCount) >= minCount);
                    break;

                case "total":
                    query = query.Where(c => (c.ByName.InventoryCount + c.ByName.DeckCount) >= minCount);
                    break;
            }

            var queryResult = await query.Select(c => new TrimmingToolResult
            {
                Id = c.ByUnique.Id,
                CardId = c.ByUnique.CardId,
                SetCode = c.ByUnique.SetCode,
                Name = c.ByUnique.Name,
                ImageUrl = c.ByUnique.ImageUrl,
                CollectorNumber = c.ByUnique.CollectorNumber,

                Type = c.ByUnique.Type,
                ColorIdentity = c.ByUnique.ColorIdentity,

                IsFoil = c.ByUnique.IsFoil,

                Price = c.ByUnique.Price,
                PriceFoil = c.ByUnique.PriceFoil,
                TixPrice = c.ByUnique.TixPrice,

                PrintTotalCount = c.ByUnique.TotalCount,
                PrintDeckCount = c.ByUnique.DeckCount,
                PrintInventoryCount = c.ByUnique.InventoryCount,
                PrintSellCount = c.ByUnique.SellCount,

                AllTotalCount = c.ByName.TotalCount,
                AllDeckCount = c.ByName.DeckCount,
                AllInventoryCount = c.ByName.InventoryCount,
                AllSellCount = c.ByName.SellCount,

            }).ToListAsync();

            return queryResult;
        }

        public async Task TrimCards(List<TrimmedCardDto> cardsToTrim)
        {
            //for each card, need to get all matching inventory cards

            //get all ids
            var allIds = cardsToTrim.Select(c => c.CardId).ToArray();

            //get a dictionary of all inventory cards with a card id contained in a provided list
            var relevantCardsById = (await _cardContext.InventoryCards
                .Where(ic => ic.InventoryCardStatusId == 1 && ic.DeckCards.Count == 0 && allIds.Contains(ic.CardId))

                //includes
                .ToListAsync())
                .GroupBy(ic => ic.CardId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach(var card in cardsToTrim)
            {
                //  Get matching card by id/foil, ensure enough inventory cards exist to meet the request, update the cards along the way

                var relevantInventoryCards = relevantCardsById[card.CardId].Where(c => c.IsFoil == card.IsFoil).ToArray();

                if(relevantInventoryCards.Length < card.NumberToTrim)
                {
                    //Should never attempt to trim more cards than exist.  Attempting to do so is a red flag
                    throw new Exception($"Not enough unused cards to trim. Card Name: {card.CardName}, Number to trim: {card.NumberToTrim}, Available: {relevantInventoryCards.Length}");
                }

                for(int i = 0; i < card.NumberToTrim; i++)
                {
                    var cardToUpdate = relevantInventoryCards[i];
                    cardToUpdate.InventoryCardStatusId = 3;
                    _cardContext.InventoryCards.Update(cardToUpdate);
                }
            }

            //disabling this for now until the UI appears to be working again
            //int breakpoint = 1;
            await _cardContext.SaveChangesAsync();
        }

        #endregion

        //get collection/inventory totals by status
        // GetTotalsByStatus | GetCollectionTotals | GetInventoryTotals
        public async Task<IEnumerable<InventoryTotalsByStatusResult>> GetCollectionTotals()
        {
            var result = await _cardContext.InventoryTotalsByStatus.ToListAsync();
            //Should really return a DTO instead but whatever
            return result;
        }
    }
}
