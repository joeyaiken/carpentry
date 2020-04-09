using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class InventoryService : IInventoryService
    {

        public InventoryService()
        {

        }

        #region private methods

        //        private async Task<decimal?> CalculateInventoryTotalPrice()
        //        {
        //            //Should this refresh scryfall prices?
        //            //TODO - refresh scryfall prices when querying this


        //            var priceQuery = _cardRepo.QueryInventoryCards() //_cardContext.DeckCards
        //                .Select(x => new
        //                {
        //                    x.Id,
        //                    x.Card.Variants.Where(cardVariant => cardVariant.CardVariantTypeId == x.VariantTypeId).FirstOrDefault().Price,
        //                });

        //            var totalPrice = await priceQuery.SumAsync(x => x.Price);

        //            return totalPrice;
        //        }

        //        private async Task<DeckStats> GetDeckStats(int deckId)
        //        {
        //            DeckStats result = new DeckStats();

        //            var deckProps = await _cardRepo.QueryDeckProperties().FirstOrDefaultAsync(x => x.Id == deckId);

        //            //total Count
        //            var deckCardCount = await _cardRepo.QueryDeckCards().Where(x => x.DeckId == deckId && x.CategoryId != 's').CountAsync();

        //            int basicLandCount = deckProps.BasicW + deckProps.BasicU + deckProps.BasicB + deckProps.BasicR + deckProps.BasicG;

        //            result.TotalCount = deckCardCount + basicLandCount;

        //            //total price
        //            var priceQuery = _cardRepo.QueryDeckCards() //_cardContext.DeckCards
        //                .Where(x => x.DeckId == deckId && x.CategoryId != 's')
        //                .Select(x => new
        //                {
        //                    CardName = x.InventoryCard.Card.Name,
        //                    //CardVariantName = x.InventoryCard.VariantType.Name,
        //                    //VariantTypeId = x.InventoryCard.VariantTypeId,
        //                    ////price
        //                    Price = x.InventoryCard.Card.Variants.Where(cardVariant => cardVariant.CardVariantTypeId == x.InventoryCard.VariantTypeId).FirstOrDefault().Price,

        //                });

        //            var totalPrice = await priceQuery.SumAsync(x => x.Price);

        //            result.TotalCost = totalPrice ?? 0;

        //            //type-breakdown
        //            var cardTypes = _cardRepo.QueryDeckCards()
        //                .Where(x => x.DeckId == deckId && x.CategoryId != 's')
        //                .Select(x => x.InventoryCard.Card.Type)
        //                .Select(x => GetCardTypeGroup(x))
        //                .ToList();

        //            var typeCountsDict = cardTypes
        //                .GroupBy(x => x)
        //                .Select(x => new
        //                {
        //                    Name = x.Key,
        //                    Count = x.Count()
        //                })
        //                .AsEnumerable()
        //                .ToDictionary(x => x.Name, x => x.Count);

        //            if (typeCountsDict.Keys.Contains("Lands"))
        //            {
        //                typeCountsDict["Lands"] = typeCountsDict["Lands"] + basicLandCount;
        //            }
        //            else
        //            {
        //                typeCountsDict["Lands"] = basicLandCount;
        //            }

        //            result.TypeCounts = typeCountsDict;


        //            //cost-breakdown, probably requires re-querying everything
        //            var deckCardCostsDict = _cardRepo.QueryDeckCards() //_cardContext.DeckCards
        //                .Where(x => x.DeckId == deckId && x.CategoryId != 's')
        //                .Select(x => x.InventoryCard.Card)
        //                .Where(x => !x.Type.Contains("Land"))
        //                .Select(x => x.Cmc)
        //                //.ToList();
        //                .GroupBy(x => x)
        //                .Select(x => new
        //                {
        //                    Name = x.Key,
        //                    Count = x.Count(),
        //                })
        //                .AsEnumerable()
        //                .OrderByDescending(x => x.Name)
        //                .ToDictionary(x => x.Name.ToString(), x => x.Count);

        //            result.CostCounts = deckCardCostsDict;


        //            //deck color identity
        //            //all of the basic lands 
        //            //+ every card's color identity
        //            var cardCIQuery = _cardRepo.QueryDeckCards() //_cardContext.DeckCards
        //                .Where(x => x.DeckId == deckId)
        //                .Select(x => x.InventoryCard.Card)
        //                .SelectMany(card => card.CardColorIdentities.Select(ci => ci.ManaTypeId))
        //                .Distinct();

        //            result.ColorIdentity = cardCIQuery.ToList();

        //            return result;
        //        }

        //        private static string GetCardTypeGroup(string cardType)
        //        {
        //            if (cardType.ToLower().Contains("creature"))
        //            {
        //                return "Creatures";
        //            }
        //            else if (cardType.ToLower().Contains("land"))
        //            {
        //                return "Lands";
        //            }
        //            else if (cardType.ToLower().Contains("planeswalker"))
        //            {
        //                return "Planeswalkers";
        //            }
        //            else if (cardType.ToLower().Contains("enchantment"))
        //            {
        //                return "Enchantments";
        //            }
        //            else if (cardType.ToLower().Contains("artifact"))
        //            {
        //                return "Artifacts";
        //            }
        //            //else if (cardType.ToLower().Contains(""))
        //            //{
        //            //    return "";
        //            //}
        //            //else if (cardType.ToLower().Contains(""))
        //            //{
        //            //    return "";
        //            //}
        //            else
        //            {
        //                return "Spells";
        //            }
        //        }

        //        public async Task EnsureCardDefinitionExists(int multiverseId)
        //        {
        //            var dbCard = await _cardRepo.QueryCardDefinitions().FirstOrDefaultAsync(x => x.Id == multiverseId);

        //            if (dbCard != null)
        //            {
        //                return;
        //            }

        //            var scryfallCard = await _scryRepo.GetCardById(multiverseId);

        //            await _cardRepo.AddCardDefinition(scryfallCard);
        //            //_logger.LogWarning($"EnsureCardDefinitionExists added {multiverseId} - {scryfallCard.Name}");
        //        }

        //        private static IQueryable<ScryfallMagicCard> MapInventoryQueryToScryfallDto(IQueryable<Data.DataContext.Card> query)
        //        {
        //            IQueryable<ScryfallMagicCard> result = query.Select(card => new ScryfallMagicCard()
        //            {
        //                Cmc = card.Cmc,
        //                ManaCost = card.ManaCost,
        //                MultiverseId = card.Id,
        //                Name = card.Name,

        //                //Prices = card.Variants.ToDictionary(v => (v.)  )

        //                Prices = card.Variants.SelectMany(x => new[]
        //                {
        //                    new {
        //                        Name = x.Type.Name,
        //                        Price = x.Price,
        //                    },
        //                    new {
        //                        Name = $"{x.Type.Name}_foil",
        //                        Price = x.PriceFoil,
        //                    }
        //                }).ToDictionary(v => v.Name, v => v.Price),

        //                //Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
        //                Variants = card.Variants.Select(v => new { v.Type.Name, v.ImageUrl }).ToDictionary(v => v.Name, v => v.ImageUrl),
        //                Colors = card.CardColors.Select(c => c.ManaType.Name).ToList(),
        //                Rarity = card.Rarity.Name,
        //                Set = card.Set.Code,
        //                Text = card.Text,
        //                Type = card.Type,
        //                ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList(),
        //                Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
        //            });
        //            return result;
        //        }

        //        private async Task<string> ValidateDeck(int deckId)
        //        {
        //            string validationResult = "";
        //            List<string> validationErrors = new List<string>();

        //            var deck = await _cardRepo.QueryDeckProperties().Where(x => x.Id == deckId).FirstOrDefaultAsync();

        //            string deckFormat = deck.Format.ToLower();

        //            #region Validate deck size

        //            int basicLandCount = deck.BasicW + deck.BasicU + deck.BasicB + deck.BasicR + deck.BasicG;

        //            var cardCount = _cardRepo.QueryDeckCards().Where(x => x.DeckId == deckId).Count();

        //            int deckSize = basicLandCount + cardCount;

        //            //what's the min deck count for this format?
        //            if (deckFormat == "commander")
        //            {
        //                //must be exactly 100 cards to be valid
        //                if (deckSize < 100)
        //                {
        //                    validationErrors.Add($"Below size requirement: {deckSize}/100 cards");
        //                }

        //                if (deckSize > 100)
        //                {
        //                    validationErrors.Add($"Above size limit: {deckSize}/100 cards");
        //                }
        //            }
        //            else
        //            {
        //                if ((deckFormat == "brawl" || deckFormat == "oathbreaker") && deckSize > 60)
        //                {
        //                    validationErrors.Add($"Above size limit: {deckSize}/60 cards");
        //                }

        //                if (deckSize < 60)
        //                {
        //                    validationErrors.Add($"Below size requirement: {deckSize}/60 cards");
        //                }
        //            }

        //            #endregion

        //            #region Validate max per card (1 for singleton, 4 for other formats)



        //            #endregion

        //            #region Validate format legality



        //            #endregion

        //            #region Validate color rules for commander/brawl



        //            #endregion

        //            validationResult = string.Join(" ", validationErrors);

        //            return validationResult;
        //        }

        #endregion

        #region Inventory related methods

        public async Task<int> AddInventoryCard(InventoryCard dto)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
            //await EnsureCardDefinitionExists(dto.MultiverseId);

            //var newId = await _cardRepo.AddInventoryCard(dto);

            //return newId;
        }

        public async Task AddInventoryCardBatch(IEnumerable<InventoryCard> cards)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
            ////Ensure all cards exist in the repo
            ////Will this break when multiple cards are called for the same set?
            //var cardRequests = cards.Select(x => x.MultiverseId).Distinct().Select(mid => EnsureCardDefinitionExists(mid)).ToList();
            //await Task.WhenAll(cardRequests);

            //await _cardRepo.AddInventoryCardBatch(cards);
        }

        public async Task UpdateInventoryCard(InventoryCard dto)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
            //await _cardRepo.UpdateInventoryCard(dto);
        }

        public async Task DeleteInventoryCard(int id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
            //await _cardRepo.DeleteInventoryCard(id);
        }

        public async Task<IEnumerable<InventoryOverview>> GetInventoryOverviews(InventoryQueryParameter param)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();

            //if (param.GroupBy.ToLower() == "quantity")
            //{

            //    var inventoryQuery = await _cardRepo.QueryInventoryOverviews(param);

            //    //have overviews, now I need to sort things
            //    //wait I should just filter BS by color


            //    //if (param.Sort == "count")
            //    //{
            //    //    inventoryQuery = inventoryQuery.OrderByDescending(x => x.Count);
            //    //}
            //    //else if (param.Sort == "name")
            //    //{
            //    //    inventoryQuery = inventoryQuery.OrderBy(x => x.Name);
            //    //}
            //    //else if (param.Sort == "cmc")
            //    //{
            //    //    inventoryQuery = inventoryQuery.OrderBy(x => x.Cost);
            //    //}
            //    //else
            //    //{
            //    //    inventoryQuery = inventoryQuery.OrderByDescending(x => x.Count);
            //    //}


            //    //var query = inventoryQuery.OrderByDescending(x => x.Count);

            //    if (param.Take > 0)
            //    {
            //        //should eventually consider pagination here

            //        inventoryQuery = inventoryQuery.Skip(param.Skip).Take(param.Take);//.OrderByDescending(x => x.Count);
            //    }

            //    var result = await inventoryQuery.ToListAsync();

            //    return result;

            //    //This query is still missing Type & Cost vals (is cost really useful here?)

            //    //take the top X results, then get the rest of the details

            //    //approach 2 - start with inventory cards


            //}
            //else
            //{
            //    return null;
            //}

            ////if grouping by name...IDK yet


        }

        public async Task<InventoryDetail> GetInventoryDetailByName(string name)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
            //InventoryDetailDto result = new InventoryDetailDto()
            //{
            //    Name = name,
            //    Cards = new List<ScryfallMagicCard>(),
            //    InventoryCards = new List<InventoryCardDto>(),
            //};

            //var inventoryCardsQuery = _cardRepo.QueryCardDefinitions().Where(x => x.Name == name)
            //    .SelectMany(x => x.InventoryCards)
            //    .Select(x => new InventoryCardDto()
            //    {
            //        Id = x.Id,
            //        IsFoil = x.IsFoil,
            //        InventoryCardStatusId = x.InventoryCardStatusId,
            //        MultiverseId = x.MultiverseId,
            //        VariantType = x.VariantType.Name,
            //        Name = x.Card.Name,
            //        Set = x.Card.Set.Code,
            //        DeckCards = x.DeckCards.Select(c => new InventoryDeckCardDto
            //        {
            //            Id = c.Id,
            //            DeckId = c.DeckId,
            //            InventoryCardId = c.InventoryCardId,
            //            DeckName = c.Deck.Name,
            //        }).ToList()
            //    })
            //    .OrderBy(x => x.Id);

            //result.InventoryCards = await inventoryCardsQuery.ToListAsync();

            //var cardDefinitionsQuery = _cardRepo.QueryCardDefinitions().Where(x => x.Name == name);

            //result.Cards = await MapInventoryQueryToScryfallDto(cardDefinitionsQuery).ToListAsync();

            //return result;
        }

        #endregion

    }
}
