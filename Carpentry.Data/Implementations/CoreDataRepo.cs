using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Implementations
{
    public class CoreDataRepo : ICoreDataRepo
    {
        private readonly ILogger<CoreDataRepo> _logger;
        private readonly CarpentryDataContext _cardContext;
        public CoreDataRepo(CarpentryDataContext cardContext, ILogger<CoreDataRepo> logger)
        {
            _cardContext = cardContext;
            _logger = logger;
        }

        #region data reference values

        public async Task<DataReferenceValue<int>> GetMagicFormat(string formatName)
        {
            DataReferenceValue<int> matchingFormat = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower())
                .Select(x => new DataReferenceValue<int>()
                {
                    Id = x.FormatId,
                    Name = x.Name,
                }).FirstOrDefaultAsync();

            return matchingFormat;
        }

        public async Task<DataReferenceValue<int>> GetMagicFormat(int formatId)
        {
            DataReferenceValue<int> matchingFormat = await _cardContext.MagicFormats.Where(x => x.FormatId == formatId)
                .Select(x => new DataReferenceValue<int>()
                {
                    Id = x.FormatId,
                    Name = x.Name,
                }).FirstOrDefaultAsync();

            return matchingFormat;
        }

        public async Task<IEnumerable<DataReferenceValue<int>>> GetAllMagicFormats()
        {
            List<DataReferenceValue<int>> results = await _cardContext.MagicFormats
                .Select(x => new DataReferenceValue<int>()
                {
                    Id = x.FormatId,
                    Name = x.Name,
                }).ToListAsync();

            return results;
        }

        //public async Task<DataReferenceValue<int>> GetCardVariantTypeByName(string name)
        //{
        //    var result = await _cardContext.VariantTypes.FirstOrDefaultAsync(x => x.Name == name);
        //    DataReferenceValue<int> mappedResult = new DataReferenceValue<int>
        //    {
        //        Id = result.Id,
        //        Name = result.Name,
        //    };
        //    return mappedResult;
        //}

        //public async Task<List<DataReferenceValue<int>>> GetAllCardVariantTypes()
        //{
        //    List<DataReferenceValue<int>> result = await _cardContext.VariantTypes.Select(x => new DataReferenceValue<int>()
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //    }).ToListAsync();
        //    return result;
        //}

        //public async Task<IEnumerable<DataReferenceValue<char>>> GetAllManaColors()
        //{
        //    List<DataReferenceValue<char>> results = await _cardContext.ManaTypes
        //        .Select(x => new DataReferenceValue<char>()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //        }).ToListAsync();

        //    return results;
        //}

        public async Task<IEnumerable<DataReferenceValue<char>>> GetAllRarities()
        {
            List<DataReferenceValue<char>> results = await _cardContext.Rarities
                .Select(x => new DataReferenceValue<char>()
                {
                    Id = x.RarityId,
                    Name = x.Name,
                }).ToListAsync();

            return results;
        }

        public async Task<IEnumerable<DataReferenceValue<string>>> GetAllSets()
        {
            List<DataReferenceValue<string>> results = await _cardContext.Sets
                .Where(s => s.IsTracked)
                .OrderByDescending(s => s.ReleaseDate)
                .Select(x => new DataReferenceValue<string>()
                {
                    Id = x.Code,
                    Name = x.Name,
                }).ToListAsync();

            return results;
        }

        public async Task<IEnumerable<DataReferenceValue<int>>> GetAllStatuses()
        {
            List<DataReferenceValue<int>> results = await _cardContext.CardStatuses
                .Select(x => new DataReferenceValue<int>()
                {
                    Id = x.CardStatusId,
                    Name = x.Name,
                }).ToListAsync();

            return results;
        }

        public List<DataReferenceValue<string>> GetAllTypes()
        {
            List<DataReferenceValue<string>> results = new List<DataReferenceValue<string>>()
            {
                new DataReferenceValue<string>(){ Id = "Creature", Name = "Creature" },
                new DataReferenceValue<string>(){ Id = "Instant", Name = "Instant" },
                new DataReferenceValue<string>(){ Id = "Sorcery", Name = "Sorcery" },
                new DataReferenceValue<string>(){ Id = "Enchantment", Name = "Enchantment" },
                new DataReferenceValue<string>(){ Id = "Land", Name = "Land" },
                new DataReferenceValue<string>(){ Id = "Planeswalker", Name = "Planeswalker" },
                new DataReferenceValue<string>(){ Id = "Artifact", Name = "Artifact" },
                new DataReferenceValue<string>(){ Id = "Legendary", Name = "Legendary" },
            };

            return results;
        }

        #endregion

        #region TryAdd methods

        public async Task TryAddInventoryCardStatus(InventoryCardStatusData status)
        {
            var existingRecord = _cardContext.CardStatuses.FirstOrDefault(x => x.Name == status.Name);
            if (existingRecord == null)
            {
                _logger.LogWarning($"Adding card status {status.Name}");
                _cardContext.CardStatuses.Add(status);
            }
            await _cardContext.SaveChangesAsync();
        }

        public async Task TryAddCardRarity(CardRarityData rarity)
        {
            var existingRecord = _cardContext.Rarities.FirstOrDefault(x => x.RarityId == rarity.RarityId);
            if (existingRecord == null)
            {
                _logger.LogWarning($"Adding card rarity {rarity.Name}");
                _cardContext.Rarities.Add(rarity);
            }
            await _cardContext.SaveChangesAsync();
        }

        //public async Task TryAddManaType(ManaTypeData type)
        //{
        //    var existingRecord = _cardContext.ManaTypes.FirstOrDefault(x => x.Id == type.Id);
        //    if (existingRecord == null)
        //    {
        //        _logger.LogWarning($"Adding mana type {type.Name}");
        //        _cardContext.ManaTypes.Add(type);
        //        await _cardContext.SaveChangesAsync();
        //    }
        //}

        public async Task TryAddMagicFormat(MagicFormatData format)
        {
            var existingRecord = _cardContext.MagicFormats.FirstOrDefault(x => x.Name == format.Name);
            if (existingRecord == null)
            {
                _cardContext.MagicFormats.Add(format);
                _logger.LogWarning($"Adding format {format.Name}");
                await _cardContext.SaveChangesAsync();
            }
        }

        //public async Task TryAddCardVariantType(CardVariantTypeData variant)
        //{
        //    var existingRecord = _cardContext.VariantTypes.FirstOrDefault(x => x.Name == variant.Name);
        //    if (existingRecord == null)
        //    {
        //        _cardContext.VariantTypes.Add(variant);
        //        _logger.LogWarning($"Adding variant {variant.Name}");
        //        await _cardContext.SaveChangesAsync();
        //    }

        //}

        public async Task TryAddDeckCardCategory(DeckCardCategoryData category)
        {
            var existingRecord = _cardContext.DeckCardCategories.FirstOrDefault(x => x.DeckCardCategoryId == category.DeckCardCategoryId);
            if (existingRecord == null)
            {
                _cardContext.DeckCardCategories.Add(category);
                _logger.LogWarning($"Adding category {category.Name}");
            }
            await _cardContext.SaveChangesAsync();
        }

        #endregion

        public async Task EnsureDatabaseExists()
        {
            //await _cardContext.Database.EnsureDeletedAsync();
            await _cardContext.Database.EnsureCreatedAsync();

            await ExecuteSqlScript("vwAllInventoryCards");
            await ExecuteSqlScript("vwInventoryCardsByName");
            await ExecuteSqlScript("vwInventoryCardsByPrint");
            await ExecuteSqlScript("vwInventoryCardsByUnique");
            await ExecuteSqlScript("vwInventoryTotalsByStatus");
            await ExecuteSqlScript("vwSetTotals");


            await ExecuteSqlScript("spGetInventoryTotals");
            await ExecuteSqlScript("spGetTotalTrimCount");
            await ExecuteSqlScript("spGetTrimmingTips");

            //await _cardContext.SaveChangesAsync();


        }
        private async Task ExecuteSqlScript(string scriptName)
        {
            try
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;

                var something = Directory.GetCurrentDirectory();
                //Directory.
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                var baseDir = Path.GetDirectoryName(path) + $"\\DataScripts\\{scriptName}.sql";
                var scriptContents = File.ReadAllText(baseDir);

#pragma warning disable CS0618 // Type or member is obsolete
                await _cardContext.Database.ExecuteSqlCommandAsync(scriptContents);
#pragma warning restore CS0618 // Type or member is obsolete
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error attempting to add DB object {scriptName}", ex);
            }
            
        }

    }
}
