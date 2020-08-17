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

        public async Task<AppFiltersDto> GetAppFilterValues()
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
                new FilterOption()
                {
                    Value = "W",
                    Name = "White",
                },
                new FilterOption()
                {
                    Value = "U",
                    Name = "Blue",
                },
                new FilterOption()
                {
                    Value = "B",
                    Name = "Black",
                },
                new FilterOption()
                {
                    Value = "R",
                    Name = "Red",
                },
                new FilterOption()
                {
                    Value = "G",
                    Name = "Green",
                },
            };

            result.Colors = allManaTypes;

            return result;
        }

    }
}
