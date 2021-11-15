using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Microsoft.Extensions.Logging;

namespace Carpentry.Logic
{
    public class NewInventoryCard
    {
        public int CardId { get; set; }
        public int StatusId { get; set; }
        public bool IsFoil { get; set; }
    }
    
    public interface IInventoryService
    {
        //Inventory Card add/update/delete
        Task<int> AddInventoryCard(InventoryCardDto dto);
        Task AddInventoryCardBatch(IEnumerable<NewInventoryCard> cards);
        Task UpdateInventoryCard(InventoryCardDto dto);
        Task UpdateInventoryCardBatch(IEnumerable<InventoryCardDto> batch);
        Task DeleteInventoryCard(int id);
        Task DeleteInventoryCardBatch(IEnumerable<int> batch);

        //Search
        //Task<List<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param);
        Task<InventoryDetailDto> GetInventoryDetail(int cardId);

        Task<List<TrimmingToolResult>> GetTrimmingToolCards(string setCode, int minCount, string filterBy, string searchGroup = null);
        Task TrimCards(List<TrimmedCardDto> cardsToTrim);

        Task<List<InventoryTotalsByStatusResult>> GetCollectionTotals();
    }

    public class InventoryService : IInventoryService
    {
        private readonly CarpentryDataContext _cardContext;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(CarpentryDataContext cardContext, ILogger<InventoryService> logger)
        {
            _cardContext = cardContext;
            _logger = logger;
        }

        public async Task<int> AddInventoryCard(InventoryCardDto dto)
        {
            //TODO - validate card ID, instead of blindly trusting it if > 0
            var newInventoryCard = new InventoryCardData()
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

                
                /// :(
                var cardId = await _cardContext.Cards
                    .Where(x => x.Set.Code.ToLower() == dto.Set.ToLower() && x.CollectorNumber == dto.CollectorNumber)
                    .Select(c => c.CardId)
                    .SingleOrDefaultAsync();

                //if (string.IsNullOrEmpty(dto.Set) || dto.CollectorNumber == 0)
                //{
                //    throw new Exception("Cannot add inventory card, not enough information to find a card");
                //}

                //If not, does the DTO contain a valid Set/CollectorNumber combo?
                //var cardData = await _cardDataRepo.GetCardData(dto.Set, dto.CollectorNumber);


                newInventoryCard.CardId = cardId;


            }



            //Is the set actually tracked?

            //Then, build a Data object and send to DB



            //await _dataUpdateService.EnsureCardDefinitionExists(dto.MultiverseId);
            //DataReferenceValue<int> cardVariant = await _coreDataRepo.GetCardVariantTypeByName(dto.VariantName);

            _cardContext.InventoryCards.Add(newInventoryCard);
            await _cardContext.SaveChangesAsync();

            return newInventoryCard.InventoryCardId;
        }
        //TODO - Use some smaller DTO that only has the 3 fields actually used
        public async Task AddInventoryCardBatch(IEnumerable<NewInventoryCard> cards)
        {

            //TODO - Ensure all cards exist in the repo
            //(OR, just things fail.  Both are equal to the client app.  The controller will log anything that happens)


            //var distinctIDs = cards.Select(x => x.MultiverseId).Distinct().ToList();

            //for (int i = 0; i < distinctIDs.Count(); i++)
            //{
            //    //await _dataUpdateService.EnsureCardDefinitionExists(distinctIDs[i]);
            //}

            //var variantTypes = await _coreDataRepo.GetAllCardVariantTypes();

            var newCards = cards.Select(x => new InventoryCardData()
            {
                IsFoil = x.IsFoil,
                InventoryCardStatusId = x.StatusId,
                CardId = x.CardId,
                //MultiverseId = x.MultiverseId,
                //VariantTypeId = variantTypes.FirstOrDefault(v => v.Name == x.VariantName).Id,

            }).ToList();

            _cardContext.InventoryCards.AddRange(newCards);
            await _cardContext.SaveChangesAsync();
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

            var dbCard = await _cardContext.InventoryCards.Where(x => x.InventoryCardId == dto.Id).FirstOrDefaultAsync();

            //currently only expecting to change the status with this method
            dbCard.InventoryCardStatusId = dto.StatusId;

            //await _inventoryRepo.UpdateInventoryCard(dbCard);
            //todo - actually check if exists? I could just let it error
            var existingCard = await _cardContext.InventoryCards.FirstOrDefaultAsync(c => c.InventoryCardId == dbCard.InventoryCardId);

            if (existingCard == null)
            {
                throw new Exception("Could not find a matching inventory card to update");
            }

            _cardContext.InventoryCards.Update(dbCard);
            await _cardContext.SaveChangesAsync();
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
            var deckCardsReferencingThisCard = _cardContext.DeckCards.Where(x => x.DeckId == id).Count();

            if (deckCardsReferencingThisCard > 0)
            {
                throw new Exception("Cannot delete a card that's currently in a deck");
            }

            var cardToRemove = _cardContext.InventoryCards.First(x => x.InventoryCardId == id);

            _cardContext.InventoryCards.Remove(cardToRemove);

            await _cardContext.SaveChangesAsync();
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

            var matchingCard = await _cardContext.Cards.FirstOrDefaultAsync(x => x.CardId == cardId);

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

            List<InventoryCardDto> inventoryCards = (await GetInventoryCardsByName(matchingCard.Name))
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

            var aQuery = await _cardContext.Cards
                .Where(x => x.Name == matchingCard.Name)
                .Include(c => c.Set)
                .Include(c => c.Legalities).ThenInclude(l => l.Format)
                .Include(c => c.Rarity)
                .ToListAsync();

            var mappedQuery = aQuery.Select(card => new MagicCardDto()
            {
                CardId = card.CardId,
                Cmc = card.Cmc,
                ManaCost = card.ManaCost,
                MultiverseId = card.MultiverseId,
                Name = card.Name,
                CollectionNumber = card.CollectorNumber,
                ImageUrl = card.ImageUrl,
                Price = (decimal?)card.Price,
                PriceFoil = (decimal?)card.PriceFoil,
                PriceTix = (decimal?)card.TixPrice,
                Colors = card.Color.Split().ToList(),//might run into errors if card is colorless
                Rarity = card.Rarity.Name,
                Set = card.Set.Code,
                Text = card.Text,
                Type = card.Type,
                ColorIdentity = card.ColorIdentity.Split().ToList(),
                Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
            }).ToList();

            result.Cards = mappedQuery;
            
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
        public async Task<List<InventoryTotalsByStatusResult>> GetCollectionTotals()
        {
            //Should attempt to replicate this view with an EF query instead
            //This just gets the total price and total count of all cards in the 3 statuses (Inventory, BuyList, SellList)
            

            //var legacyResult = await _cardContext.InventoryTotalsByStatus.ToListAsync();

            var result = await _cardContext.GetInventoryTotalsByStatus().ToListAsync();

            //Should really return a DTO instead but whatever
            return result;
        }

        /// <summary>
        /// This loads cards for "Get Inventory Detail" 
        /// </summary>
        /// <param name="cardName"></param>
        /// <returns></returns>
        private async Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName)
        {
            List<InventoryCardResult> inventoryCards = await _cardContext.Cards.Where(x => x.Name == cardName)
                .SelectMany(x => x.InventoryCards)
                .Select(x => new InventoryCardResult()
                {
                    Id = x.InventoryCardId,
                    IsFoil = x.IsFoil,
                    InventoryCardStatusId = x.InventoryCardStatusId,

                    CardId = x.CardId,
                    Name = x.Card.Name,
                    Type = x.Card.Type,
                    Set = x.Card.Set.Code,
                    CollectorNumber = x.Card.CollectorNumber,

                    DeckId = x.DeckCards.SingleOrDefault().DeckId,
                    DeckName = x.DeckCards.SingleOrDefault().Deck.Name, //this probs causes an exception
                    DeckCardId = x.DeckCards.SingleOrDefault().DeckCardId,
                    DeckCardCategory = x.DeckCards.SingleOrDefault().CategoryId,


                    //MultiverseId = x.MultiverseId,
                    //VariantType = x.VariantType.Name,


                    //DeckCards = x.DeckCards.Select(c => new DeckCardResult()
                    //{
                    //    Id = c.Id,
                    //    DeckId = c.DeckId,
                    //    InventoryCardId = c.InventoryCardId,
                    //    DeckName = c.Deck.Name,
                    //}).ToList()
                })
                .OrderBy(x => x.Id)
                .ToListAsync();

            return inventoryCards;

        }


    }
}
