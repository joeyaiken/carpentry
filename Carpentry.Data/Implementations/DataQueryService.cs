using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Data.QueryResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Carpentry.Data.Implementations
{
    /// <summary>
    /// This class will preform all of the overview/detail queries on the DB
    /// It will NOT return DataModel classes
    /// It will return QueryResults classes
    /// It will accept single parameters, either default types, or something in the QueryParameters folder
    /// </summary>

    public class DataQueryService //: IDataQueryService
    {
        private readonly CarpentryDataContext _cardContext;
        private readonly ILogger<DataQueryService> _logger;

        public DataQueryService(CarpentryDataContext cardContext, ILogger<DataQueryService> logger)
        {
            _cardContext = cardContext;
            _logger = logger;
        }

        #region private

        private static IQueryable<CardDataDto> MapInventoryQueryToScryfallDto(IQueryable<CardData> query)
        {
            IQueryable<CardDataDto> result = query.Select(card => new CardDataDto()
            {
                Cmc = card.Cmc,
                ManaCost = card.ManaCost,
                MultiverseId = card.Id,
                Name = card.Name,
                Variants = card.Variants.Select(v => new CardVariantDto()
                {
                    Image = v.ImageUrl,
                    Name = v.Type.Name,
                    Price = v.Price,
                    PriceFoil = v.PriceFoil,
                }).ToList(),
                //Prices = card.Variants.ToDictionary(v => (v.)  )

                //Prices = card.Variants.SelectMany(x => new[]
                //{
                //            new {
                //                Name = x.Type.Name,
                //                Price = x.Price,
                //            },
                //            new {
                //                Name = $"{x.Type.Name}_foil",
                //                Price = x.PriceFoil,
                //            }
                //        }).ToDictionary(v => v.Name, v => v.Price),

                //Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
                //Variants = card.Variants.Select(v => new { v.Type.Name, v.ImageUrl }).ToDictionary(v => v.Name, v => v.ImageUrl),
                Colors = card.CardColors.Select(c => c.ManaType.Name).ToList(),
                Rarity = card.Rarity.Name,
                Set = card.Set.Code,
                //SetId = card.Set.Id,
                Text = card.Text,
                Type = card.Type,
                ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList(),
                Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
            });
            return result;
        }

        private async Task<IQueryable<CardData>> QueryFilteredCards(InventoryQueryParameter filters)
        {
            var cardsQuery = _cardContext.Cards.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Set))
            {
                var matchingSetId = _cardContext.Sets.Where(x => x.Code.ToLower() == filters.Set.ToLower()).Select(x => x.Id).FirstOrDefault();
                cardsQuery = cardsQuery.Where(x => x.SetId == matchingSetId);
            }

            if (filters.StatusId > 0)
            {
                //cardsQuery = cardsQuery.Where(x => x.)
            }

            if (filters.Colors != null && filters.Colors.Any())
            {
                //var allowedColorIDs = filters.Colors.

                var excludedColors = await _cardContext.ManaTypes.Where(x => !filters.Colors.Contains(x.Id.ToString())).Select(x => x.Id).ToListAsync();

                //var includedColors = filters.Colors;

                //only want cards where every color is an included color
                //cardsQuery = cardsQuery.Where(x => !x.CardColorIdentities.Any() || x.CardColorIdentities.Any(color => includedColors.Contains(color.ManaTypeId.ToString())));

                //alternative query, no excluded colors
                cardsQuery = cardsQuery.Where(x => !(x.CardColorIdentities.Any(color => excludedColors.Contains(color.ManaTypeId))));

            }

            if (!string.IsNullOrEmpty(filters.Format))
            {
                //var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
                var matchingFormatId = await GetFormatIdByName(filters.Format);
                cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
            }

            if (filters.ExclusiveColorFilters)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() == filters.Colors.Count());
            }

            if (filters.MultiColorOnly)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() > 1);
            }

            if (!string.IsNullOrEmpty(filters.Type))
            {
                cardsQuery = cardsQuery.Where(x => x.Type.Contains(filters.Type));
            }

            if (filters.Rarity != null && filters.Rarity.Any())
            {
                cardsQuery = cardsQuery.Where(x => filters.Rarity.Contains(x.Rarity.Name.ToLower()));

            }

            if (!string.IsNullOrEmpty(filters.Text))
            {
                cardsQuery = cardsQuery.Where(x =>
                    x.Text.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Name.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Type.ToLower().Contains(filters.Text.ToLower())
                );
            }

            return cardsQuery;
        }

        private async Task<int> GetFormatIdByName(string formatName)
        {
            var format = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower()).FirstOrDefaultAsync();
            if (format == null)
            {
                throw new Exception($"Could not find format matching name: {formatName}");
            }
            return format.Id;
        }

        #endregion

        #region Inventory

        


        #endregion

    }
}
