using Carpentry.DataLogic;
using Carpentry.Logic.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Logic
{
    public interface IFilterService
    {
        Task<AppFiltersDto> GetAppCoreData();
        Task<List<FilterOption>> GetFormatFilters();
        Task<List<FilterOption>> GetRarityFilters();
        Task<List<FilterOption>> GetCardSetFilters();
        Task<List<FilterOption>> GetStatusFilters();
        List<FilterOption> GetCardTypeFilters();
        List<FilterOption> GetManaTypeFilters();
        List<FilterOption> GetCardGroupFilters();
        List<FilterOption> GetInventoryGroupOptions();
        List<FilterOption> GetInventorySortOptions();
    }

    public class FilterService : IFilterService
    {
        private readonly ICoreDataRepo _coreDataRepo;

        public FilterService(ICoreDataRepo coreDataRepo)
        {
            _coreDataRepo = coreDataRepo;
        }

        public async Task<AppFiltersDto> GetAppCoreData()
        {
            var result = new AppFiltersDto
            {
                Formats = await GetFormatFilters(),
                Rarities = await GetRarityFilters(),
                Sets = await GetCardSetFilters(),
                Statuses = await GetStatusFilters(),
                Types = GetCardTypeFilters(),
                Colors = GetManaTypeFilters(),
                SearchGroups = GetCardGroupFilters(),
                GroupBy = GetInventoryGroupOptions(),
                SortBy = GetInventorySortOptions(),
            };

            return result;
        }

        public async Task<List<FilterOption>> GetFormatFilters()
        {
            return (await _coreDataRepo.GetAllMagicFormats())
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                }).ToList();
        }

        public async Task<List<FilterOption>> GetRarityFilters()
        {
            return (await _coreDataRepo.GetAllRarities())
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                }).ToList();
        }
    
        public async Task<List<FilterOption>> GetCardSetFilters()
        {
            return (await _coreDataRepo.GetAllSets())
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                }).ToList();
        }

        public async Task<List<FilterOption>> GetStatusFilters()
        {
            return (await _coreDataRepo.GetAllStatuses())
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                }).ToList();
        }

        public List<FilterOption> GetCardTypeFilters()
        {
            return new List<FilterOption>()
            {
                new FilterOption(){ Value = "Creature", Name = "Creature" },
                new FilterOption(){ Value = "Instant", Name = "Instant" },
                new FilterOption(){ Value = "Sorcery", Name = "Sorcery" },
                new FilterOption(){ Value = "Enchantment", Name = "Enchantment" },
                new FilterOption(){ Value = "Land", Name = "Land" },
                new FilterOption(){ Value = "Planeswalker", Name = "Planeswalker" },
                new FilterOption(){ Value = "Artifact", Name = "Artifact" },
                new FilterOption(){ Value = "Legendary", Name = "Legendary" },
            };
        }

        public List<FilterOption> GetManaTypeFilters()
        {
            return new List<FilterOption>()
            {
                new FilterOption() {Value = "W", Name = "White"},
                new FilterOption() {Value = "U",Name = "Blue"},
                new FilterOption() {Value = "B",Name = "Black"},
                new FilterOption() {Value = "R",Name = "Red"},
                new FilterOption() {Value = "G",Name = "Green"},
            };
        }

        public List<FilterOption> GetCardGroupFilters()
        {
            return new List<FilterOption>()
            {
                new FilterOption() { Value = "Red", Name = "Red" },
                new FilterOption() { Value = "Blue", Name = "Blue" },
                new FilterOption() { Value = "Green", Name = "Green" },
                new FilterOption() { Value = "White", Name = "White" },
                new FilterOption() { Value = "Black", Name = "Black" },
                new FilterOption() { Value = "Multicolored", Name = "Multicolored" },
                new FilterOption() { Value = "Colorless", Name = "Colorless" },
                new FilterOption() { Value = "Lands", Name = "Lands" },
                new FilterOption() { Value = "RareMythic", Name = "Rare/Mythic" },
            };
        }

        public List<FilterOption> GetInventoryGroupOptions()
        {
            return new List<FilterOption>()
            {
                new FilterOption() { Value = "name", Name = "Name" },
                new FilterOption() { Value = "print", Name = "Print" },
                new FilterOption() { Value = "unique", Name = "Unique" },
            };
        }

        public List<FilterOption> GetInventorySortOptions()
        {
            return new List<FilterOption>()
            {
                new FilterOption() { Value = "count", Name = "Count" },
                new FilterOption() { Value = "name", Name = "Name" },
                new FilterOption() { Value = "price", Name = "Price" },
                new FilterOption() { Value = "cmc", Name = "Cmc" },
                new FilterOption() { Value = "collectorNumber", Name = "Collector Number"}
            };
        }
    }
}
