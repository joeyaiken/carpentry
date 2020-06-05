using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Implementations
{
    public class DataReferenceService : IDataReferenceService
    {
        //readonly ScryfallDataContext _scryContext;
        private readonly CarpentryDataContext _cardContext;
        //private readonly ILogger<DataReferenceService> _logger;

        public DataReferenceService(CarpentryDataContext cardContext
            //, ILogger<DataReferenceService> logger
            )
        {
            _cardContext = cardContext;
            //_logger = logger;
        }

        //public async Task<MagicFormat> GetFormatByName(string formatName)
        //{
        //    var matchingFormat = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower()).FirstOrDefaultAsync();
        //    return matchingFormat;
        //}
        //public async Task<MagicFormat> GetFormatById(int formatId)
        //{
        //    var matchingFormat = await _cardContext.MagicFormats.Where(x => x.Id == formatId).FirstOrDefaultAsync();
        //    return matchingFormat;
        //}

        public async Task<DataReferenceValue<int>> GetMagicFormat(string formatName)
        {
            DataReferenceValue<int> matchingFormat = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower())
                .Select(x => new DataReferenceValue<int>(){
                    Id = x.Id,
                    Name = x.Name,
                }).FirstOrDefaultAsync();
            
            return matchingFormat;
        }
        public async Task<DataReferenceValue<int>> GetMagicFormat(int formatId)
        {
            DataReferenceValue<int> matchingFormat = await _cardContext.MagicFormats.Where(x => x.Id == formatId)
                .Select(x => new DataReferenceValue<int>()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).FirstOrDefaultAsync();

            return matchingFormat;
        }
        public async Task<IEnumerable<DataReferenceValue<int>>> GetAllMagicFormats()
        {
            List<DataReferenceValue<int>> results = await _cardContext.MagicFormats
                .Select(x => new DataReferenceValue<int>()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();

            return results;
        }

        public async Task<DataReferenceValue<int>> GetCardVariantTypeByName(string name)
        {
            var result = await _cardContext.VariantTypes.FirstOrDefaultAsync(x => x.Name == name);
            DataReferenceValue<int> mappedResult = new DataReferenceValue<int>
            {
                Id = result.Id,
                Name = result.Name,
            };
            return mappedResult;
        }

        public async Task<List<DataReferenceValue<int>>> GetAllCardVariantTypes()
        {
            List<DataReferenceValue<int>> result = await _cardContext.VariantTypes.Select(x => new DataReferenceValue<int>()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<DataReferenceValue<char>>> GetAllManaColors()
        {
            List<DataReferenceValue<char>> results = await _cardContext.ManaTypes
                .Select(x => new DataReferenceValue<char>()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();

            return results;
        }

        public async Task<IEnumerable<DataReferenceValue<char>>> GetAllRarities()
        {
            List<DataReferenceValue<char>> results = await _cardContext.Rarities
                .Select(x => new DataReferenceValue<char>()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();

            return results;
        }

        public async Task<IEnumerable<DataReferenceValue<string>>> GetAllSets()
        {
            List<DataReferenceValue<string>> results = await _cardContext.Sets
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
                    Id = x.Id,
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

    }
}
