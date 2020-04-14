using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.Data.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
//using Carpentry.Data.DataContext;

namespace Carpentry.Logic.Implementations
{
    public class InventoryService : IInventoryService
    {

        private readonly IInventoryDataRepo _inventoryRepo;
        
        private readonly IDataUpdateService _dataUpdateService;




        public InventoryService()
        {

        }

        #region private methods


        private static IQueryable<MagicCard> MapInventoryQueryToMagicCardObject(IQueryable<Data.DataModels.CardData> query)
        {
            IQueryable<MagicCard> result = query.Select(card => new MagicCard()
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

            Data.DataModels.CardVariantTypeData cardVariant = await _inventoryRepo.GetCardVariantTypeByName(dto.VariantType);

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
                VariantType = await _inventoryRepo.GetCardVariantTypeByName(x.VariantType),

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

        public async Task<IEnumerable<InventoryOverview>> GetInventoryOverviews(InventoryQueryParameter param)
        {
            //#error not implemented
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
            InventoryDetail result = new InventoryDetail()
            {
                Name = name,
                Cards = new List<MagicCard>(),
                InventoryCards = new List<InventoryCard>(),
            };

            var inventoryCardsQuery = _inventoryRepo.QueryCardDefinitions().Where(x => x.Name == name)
                .SelectMany(x => x.InventoryCards)
                .Select(x => new InventoryCard()
                {
                    Id = x.Id,
                    IsFoil = x.IsFoil,
                    InventoryCardStatusId = x.InventoryCardStatusId,
                    MultiverseId = x.MultiverseId,
                    VariantType = x.VariantType.Name,
                    Name = x.Card.Name,
                    Set = x.Card.Set.Code,
                    DeckCards = x.DeckCards.Select(c => new InventoryDeckCard
                    {
                        Id = c.Id,
                        DeckId = c.DeckId,
                        InventoryCardId = c.InventoryCardId,
                        DeckName = c.Deck.Name,
                    }).ToList()
                })
                .OrderBy(x => x.Id);

            result.InventoryCards = await inventoryCardsQuery.ToListAsync();

            var cardDefinitionsQuery = _inventoryRepo.QueryCardDefinitions().Where(x => x.Name == name);

            result.Cards = await MapInventoryQueryToMagicCardObject(cardDefinitionsQuery).ToListAsync();

            return result;
        }

        #endregion

    }
}
