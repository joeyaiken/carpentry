using Carpentry.Data.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class FilterService : IFilterService
    {
        private readonly IDataReferenceService _dataReferenceService;

        public FilterService(IDataReferenceService dataReferenceService)
        {
            _dataReferenceService = dataReferenceService;
        }

        public async Task<AppFiltersDto> GetAppFilterValues()
        {
            AppFiltersDto result = new AppFiltersDto();

            var allFormats = await _dataReferenceService.GetAllMagicFormats();

            result.Formats = allFormats
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            var allRarities = await _dataReferenceService.GetAllRarities();

            result.Rarities = allRarities
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            var allSets = await _dataReferenceService.GetAllSets();

            result.Sets = allSets
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            var allStatuses = await _dataReferenceService.GetAllStatuses();

            result.Statuses = allStatuses
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();


            var allTypes = _dataReferenceService.GetAllTypes();

            result.Types = allTypes
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToLower(),
                    Name = x.Name
                })
                .ToList();

            var allManaTypes = await _dataReferenceService.GetAllManaColors();

            result.ManaColors = allManaTypes
                .Select(x => new FilterOption()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            return result;
        }

    }
}
