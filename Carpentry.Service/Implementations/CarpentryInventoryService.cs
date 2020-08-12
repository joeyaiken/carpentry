using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Service.Implementations
{
    public class CarpentryInventoryService : ICarpentryInventoryService
    {
        private readonly IInventoryService _inventoryService;
        private readonly IDataBackupService _dataBackupService;
        private readonly ICardImportService _cardImportService;
        private readonly ICollectionBuilderService _collectionBuilderService;
        private readonly ITrimmingTipsService _trimmingTipsService;

        public CarpentryInventoryService(
            IInventoryService inventoryService,
            IDataBackupService dataBackupService,
            ICardImportService cardImportService,
            ICollectionBuilderService collectionBuilderService,
            ITrimmingTipsService trimmingTipsService
        )
        {
            _inventoryService = inventoryService;
            _dataBackupService = dataBackupService;
            _cardImportService = cardImportService;
            _collectionBuilderService = collectionBuilderService;
            _trimmingTipsService = trimmingTipsService;
        }

        #region Inventory Card add/update/delete

        public async Task<int> AddInventoryCard(InventoryCardDto dto)
        {
            var response = await _inventoryService.AddInventoryCard(dto);
            return response;
        }

        public async Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards)
        {
            await _inventoryService.AddInventoryCardBatch(cards);
        }

        public async Task UpdateInventoryCard(InventoryCardDto dto)
        {
            await _inventoryService.UpdateInventoryCard(dto);
        }

        public async Task UpdateInventoryCardBatch(IEnumerable<InventoryCardDto> batch)
        {
            await _inventoryService.UpdateInventoryCardBatch(batch);
        }

        public async Task DeleteInventoryCard(int id)
        {
            await _inventoryService.DeleteInventoryCard(id);
        }

        public async Task DeleteInventoryCardBatch(IEnumerable<int> batch)
        {
            await _inventoryService.DeleteInventoryCardBatch(batch);
        }

        #endregion Inventory Card add/update/delete

        #region Search

        public async Task<List<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param)
        {
            var result = await _inventoryService.GetInventoryOverviews(param);
            return result;
        }

        public async Task<InventoryDetailDto> GetInventoryDetail(int cardId)
        {
            var result = await _inventoryService.GetInventoryDetail(cardId);
            return result;
        }

        #endregion Search

        #region Collection Builder

        public async Task<List<InventoryOverviewDto>> GetCollectionBuilderSuggestions()
        {
            var result = await _collectionBuilderService.GetCollectionBuilderSuggestions();

            return result;
        }

        public async Task HideCollectionBuilderSuggestion(InventoryOverviewDto dto)
        {
            await _collectionBuilderService.HideCollectionBuilderSuggestion(dto);
        }

        #endregion Collection Builder

        #region Trimming Tips

        public async Task<List<InventoryOverviewDto>> GetTrimmingTips()
        {
            var response = await _trimmingTipsService.GetTrimmingTips();
            return response;
        }

        public async Task HideTrimmingTip(InventoryOverviewDto dto)
        {
            await _trimmingTipsService.HideTrimmingTip(dto);
        }

        #endregion Trimming Tips

        #region Import/Export

        public async Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto cardImportDto)
        {
            var validatedPayload = await _cardImportService.ValidateCarpentryImport(cardImportDto);

            return validatedPayload;
        }

        public async Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto dto)
        {
            await _cardImportService.AddValidatedCarpentryImport(dto);
        }

        public async Task<byte[]> ExportInventoryBackup()
        {
            var result = await _dataBackupService.GenerateZipBackup();
            return result;
        }

        #endregion Import/Export
    }
}