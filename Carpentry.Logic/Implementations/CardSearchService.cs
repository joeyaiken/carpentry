﻿using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Scryfall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class CardSearchService : ICardSearchService
    {
        //Should have no access to data context classes, only repo classes
        //private readonly ICardDataRepo _cardRepo;
        private readonly IScryfallService _scryService;
        private readonly IDataQueryService _dataQueryService;
        ////private readonly ICardStringRepo _scryRepo;
        ////private readonly ILogger<CarpentryService> _logger;
        //private readonly IInventoryDataRepo _inventoryRepo;

        public CardSearchService(
            IDataQueryService dataQueryService,
            IScryfallService scryService
            
            //ICardDataRepo cardRepo
            //, ICardStringRepo scryRepo
            //, ILogger<CardSearchService> logger
            )
        {
            _dataQueryService = dataQueryService;
            _scryService = scryService;
            
            //_cardRepo = cardRepo;

            //_scryRepo = scryRepo;
            //_logger = logger;
        }

        #region private methods

        //private static IQueryable<MagicCard> MapInventoryQueryToScryfallDto(IQueryable<Data.LegacyDataContext.Card> query)
        //{
        //    IQueryable<MagicCard> result = query.Select(card => new MagicCard()
        //    {
        //        Cmc = card.Cmc,
        //        ManaCost = card.ManaCost,
        //        MultiverseId = card.Id,
        //        Name = card.Name,

        //        //Prices = card.Variants.ToDictionary(v => (v.)  )

        //        Prices = card.Variants.SelectMany(x => new[]
        //        {
        //                    new {
        //                        Name = x.Type.Name,
        //                        Price = x.Price,
        //                    },
        //                    new {
        //                        Name = $"{x.Type.Name}_foil",
        //                        Price = x.PriceFoil,
        //                    }
        //                }).ToDictionary(v => v.Name, v => v.Price),

        //        //Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
        //        Variants = card.Variants.Select(v => new { v.Type.Name, v.ImageUrl }).ToDictionary(v => v.Name, v => v.ImageUrl),
        //        Colors = card.CardColors.Select(c => c.ManaType.Name).ToList(),
        //        Rarity = card.Rarity.Name,
        //        Set = card.Set.Code,
        //        Text = card.Text,
        //        Type = card.Type,
        //        ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList(),
        //        Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
        //    });
        //    return result;
        //}

        private static MagicCardDto MapCardDataToDto(CardDataDto card)
        {
            //private static IQueryable<MagicCard> MapInventoryQueryToScryfallDto(IQueryable<Data.LegacyDataContext.Card> query)
            //{
            //    IQueryable<MagicCard> result = query.Select(card => new MagicCard()
            //    {
            //        Cmc = card.Cmc,
            //        ManaCost = card.ManaCost,
            //        MultiverseId = card.Id,
            //        Name = card.Name,

            //        Prices = card.Variants.ToDictionary(v => (v.))

            //        Prices = card.Variants.SelectMany(x => new[]
            //        {
            //                    new {
            //                        Name = x.Type.Name,
            //                        Price = x.Price,
            //                    },
            //                    new {
            //                        Name = $"{x.Type.Name}_foil",
            //                        Price = x.PriceFoil,
            //                    }
            //                }).ToDictionary(v => v.Name, v => v.Price),

            //        Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
            //        Variants = card.Variants.Select(v => new { v.Type.Name, v.ImageUrl }).ToDictionary(v => v.Name, v => v.ImageUrl),
            //        Colors = card.CardColors.Select(c => c.ManaType.Name).ToList(),
            //        Rarity = card.Rarity.Name,
            //        Set = card.Set.Code,
            //        Text = card.Text,
            //        Type = card.Type,
            //        ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList(),
            //        Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
            //    });
            //    return result;
            //}

            MagicCardDto result = new MagicCardDto()
            {
                Cmc = card.Cmc,
                ManaCost = card.ManaCost,
                MultiverseId = card.MultiverseId,
                Name = card.Name,

                //        Prices = card.Variants.ToDictionary(v => (v.))

                Prices = card.Variants.SelectMany(x => new[]
                {
                    new {
                        Name = x.Name,
                        Price = x.Price,
                    },
                    new {
                        Name = $"{x.Name}_foil",
                        Price = x.PriceFoil,
                    }
                }).ToDictionary(v => v.Name, v => v.Price),

                //        Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
                Variants = card.Variants.Select(v => new { v.Name, v.Image }).ToDictionary(v => v.Name, v => v.Image),

                Colors = card.Colors,
                Rarity = card.Rarity,
                Set = card.Set,
                Text = card.Text,
                Type = card.Type,
                ColorIdentity = card.ColorIdentity,
                Legalities = card.Legalities,
            };
            return result;
        }

        #endregion

        #region Card Search related methods

        /// <summary>
        /// Returns a list of Card Search (overview) objects, taken from cards in the inventory
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MagicCardDto>> SearchCardsFromInventory(InventoryQueryParameter filters)
        {
            //throw new NotImplementedException();

            var dbCards = await _dataQueryService.SearchInventoryCards(filters);

            List<MagicCardDto> mappedCards = dbCards.Select(x => MapCardDataToDto(x)).ToList();


            //var cardsQuery = await _inventoryRepo.QueryFilteredCards(filters);

            //var query = MapInventoryQueryToScryfallDto(cardsQuery);

            //var groupedQuery = query
            //    .GroupBy(x => x.Name)
            //    .Select(x => x.OrderByDescending(i => i.MultiverseId).First());

            //groupedQuery = groupedQuery.OrderBy(x => x.Name);

            //if (filters.Take > 0)
            //{
            //    groupedQuery = groupedQuery.Skip(filters.Skip).Take(filters.Take);
            //}

            //var result = groupedQuery.ToList();

            return mappedCards;
        }

        /// <summary>
        /// Returns all of the card definitions for a given set, with specified filters applied
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MagicCardDto>> SearchCardsFromSet(CardSearchQueryParameter filters)
        {

            var dbCards = await _dataQueryService.SearchCardSet(filters);

            List<MagicCardDto> mappedCards = dbCards.Select(x => MapCardDataToDto(x)).ToList();



            ////TODO - This filtering could/should be moved to the Data project
            //IQueryable<ScryfallMagicCard> query = await _inventoryRepo.QueryCardsBySet(filters.SetCode);

            //if (!string.IsNullOrEmpty(filters.Type))
            //{
            //    query = query.Where(x => x.Type.Contains(filters.Type));
            //}

            //filters.ColorIdentity.ForEach(color =>
            //{
            //    query = query.Where(x => x.ColorIdentity.Contains(color));
            //});

            //if (filters.ExclusiveColorFilters)
            //{
            //    query = query.Where(x => x.ColorIdentity.Count() == filters.ColorIdentity.Count());
            //}

            //if (filters.MultiColorOnly)
            //{
            //    query = query.Where(x => x.ColorIdentity.Count() > 1);
            //}

            //query = query.Where(x => filters.Rarity.Contains(x.Rarity.ToLower()));

            //List<MagicCard> result = query.OrderBy(x => x.Name).ToList();

            return mappedCards;
        }


        public async Task<IEnumerable<MagicCardDto>> SearchCardsFromWeb(NameSearchQueryParameter filters)
        {
            List<ScryfallMagicCard> queryResult = await _scryService.SearchScryfallByName(filters.Name, filters.Exclusive);

            List<MagicCardDto> mappedResult = queryResult.Select(x => x.ToMagicCard()).ToList();

            return mappedResult;
        }

        #endregion
    }
}
