using Carpentry.Data.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class FilterService : IFilterService
    {
        private readonly ICoreDataRepo _coreDataRepo;

        public FilterService(ICoreDataRepo coreDataRepo)
        {
            _coreDataRepo = coreDataRepo;
        }

        public async Task<AppFiltersDto> GetAppCoreData()
        {
            AppFiltersDto result = new AppFiltersDto();

            var allFormats = await _coreDataRepo.GetAllMagicFormats();

            result.Formats = allFormats
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            var allRarities = await _coreDataRepo.GetAllRarities();

            result.Rarities = allRarities
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            var allSets = await _coreDataRepo.GetAllSets();

            result.Sets = allSets
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            var allStatuses = await _coreDataRepo.GetAllStatuses();

            result.Statuses = allStatuses
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();


            var allTypes = _coreDataRepo.GetAllTypes();

            result.Types = allTypes
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToLower(),
                    Name = x.Name
                })
                .ToList();

            //var allManaTypes = await _coreDataRepo.GetAllManaColors();
            var allManaTypes = new List<FilterOption>()
            {
                new FilterOption() {Value = "W", Name = "White"},
                new FilterOption() {Value = "U",Name = "Blue"},
                new FilterOption() {Value = "B",Name = "Black"},
                new FilterOption() {Value = "R",Name = "Red"},
                new FilterOption() {Value = "G",Name = "Green"},
            };

            result.Colors = allManaTypes;

            var allSearchGroups = new List<FilterOption>()
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

            result.SearchGroups = allSearchGroups;

            var allGroupOptions = new List<FilterOption>()
            {
                new FilterOption() { Value = "name", Name = "Name" },
                new FilterOption() { Value = "print", Name = "Print" },
                new FilterOption() { Value = "unique", Name = "Unique" },
            };
            result.GroupBy = allGroupOptions;
            var allSortOptions = new List<FilterOption>()
            {
                new FilterOption() { Value = "count", Name = "Count" },
                new FilterOption() { Value = "name", Name = "Name" },
                new FilterOption() { Value = "price", Name = "Price" },
                new FilterOption() { Value = "cmc", Name = "Cmc" },
                new FilterOption() { Value = "collectorNumber", Name = "Collector Number"}
            };

            result.SortBy = allSortOptions;

            return result;
        }
    }
}
