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
        //private readonly IInventoryDataRepo _inventoryRepo;
        //private readonly IDataUpdateService _dataUpdateService;
        //private readonly ICoreDataRepo _coreDataRepo;
        //private readonly ICardDataRepo _cardDataRepo;
        //private readonly IDataBackupService _dataBackupService;
        //ICardImportService _cardImportService;

        ICollectionBuilderService _collectionBuilderService;
        ITrimmingTipsService _trimmingTipsService;

        public CarpentryInventoryService(
            //IInventoryDataRepo inventoryRepo,
            //IDataUpdateService dataUpdateService,
            //ICoreDataRepo coreDataRepo,
            //ICardDataRepo cardDataRepo,
            //IDataBackupService dataBackupService,
            //ICardImportService cardImportService
            ICollectionBuilderService collectionBuilderService,
            ITrimmingTipsService trimmingTipsService
        )
        {
            //_inventoryRepo = inventoryRepo;
            //_dataUpdateService = dataUpdateService;
            //_coreDataRepo = coreDataRepo;
            //_cardDataRepo = cardDataRepo;
            //_dataBackupService = dataBackupService;
            //_cardImportService = cardImportService;
            _collectionBuilderService = collectionBuilderService;
            _trimmingTipsService = trimmingTipsService;
        }

        #region Inventory Card add/update/delete

        public async Task<int> AddInventoryCard(InventoryCardDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateInventoryCard(InventoryCardDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateInventoryCardBatch(IEnumerable<InventoryCardDto> batch)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteInventoryCard(int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteInventoryCardBatch(IEnumerable<int> batch)
        {
            throw new NotImplementedException();
        }

        #endregion Inventory Card add/update/delete

        #region Search

        public async Task<List<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param)
        {
            throw new NotImplementedException();
        }

        public async Task<InventoryDetailDto> GetInventoryDetail(int cardId)
        {

            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        public async Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto dto)
        {
            throw new NotImplementedException();
        }
        public async Task<byte[]> ExportInventoryBackup()
        {
            throw new NotImplementedException();
        }

        #endregion Import/Export
    }
}