using Carpentry.Data.Interfaces;
using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Implementations
{
    public class CoreControllerService : ICoreControllerService
    {
        private readonly IDataReferenceService _dataReferenceService;

        public CoreControllerService(IDataReferenceService dataReferenceService)
        {
            _dataReferenceService = dataReferenceService;
        }

        public async Task<AppFiltersDto> GetAppFilterValues()
        {
            AppFiltersDto result = new AppFiltersDto();

            var allFormats = await _dataReferenceService.GetAllMagicFormats();

            result.Formats = allFormats
                .Select(x => new FilterOptionDto()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            var allRarities = await _dataReferenceService.GetAllRarities();
            
            result.Rarities = allRarities
                .Select(x => new FilterOptionDto()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            var allSets = await _dataReferenceService.GetAllSets();
            
            result.Sets = allSets
                .Select(x => new FilterOptionDto()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();

            var allStatuses = await _dataReferenceService.GetAllStatuses();
            
            result.Statuses = allStatuses
                .Select(x => new FilterOptionDto()
                {
                    Value = x.Id.ToString(),
                    Name = x.Name
                })
                .ToList();


            var allTypes = _dataReferenceService.GetAllTypes();
            
            result.Types = allTypes
                .Select(x => new FilterOptionDto()
                {
                    Value = x.Id.ToLower(),
                    Name = x.Name
                })
                .ToList();

            return result;
        }
    }
}
