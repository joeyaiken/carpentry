using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IInventoryService
    {
        //Inventory Card add/update/delete
        Task<int> AddInventoryCard(InventoryCardDto dto);
        Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards);
        Task UpdateInventoryCard(InventoryCardDto dto);
        Task UpdateInventoryCardBatch(IEnumerable<InventoryCardDto> batch);
        Task DeleteInventoryCard(int id);
        Task DeleteInventoryCardBatch(IEnumerable<int> batch);

        //Search
        Task<List<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param);
        Task<InventoryDetailDto> GetInventoryDetail(int cardId);

        //Collection Builder
        Task<List<InventoryOverviewDto>> GetCollectionBuilderSuggestions();
        Task HideCollectionBuilderSuggestion(InventoryOverviewDto dto);

        //Trimming Tips
        Task<List<InventoryOverviewDto>> GetTrimmingTips();
        Task HideTrimmingTip(InventoryOverviewDto dto);

        //Import/Export
        Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto cardImportDto);
        Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto dto);
        Task ExportInventoryBackup();
    }
}
