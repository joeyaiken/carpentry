using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Implementations
{
    public class DataReferenceRepo : IDataReferenceRepo
    {
        private readonly CarpentryDataContext _cardContext;
        private readonly ILogger<DataReferenceRepo> _logger;

        public DataReferenceRepo(CarpentryDataContext cardContext, ILogger<DataReferenceRepo> logger)
        {
            _cardContext = cardContext;
            _logger = logger;
        }

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
            var existingRecord = _cardContext.Rarities.FirstOrDefault(x => x.Id == rarity.Id);
            if (existingRecord == null)
            {
                _logger.LogWarning($"Adding card rarity {rarity.Name}");
                _cardContext.Rarities.Add(rarity);
            }
            await _cardContext.SaveChangesAsync();
        }
        
        public async Task TryAddManaType(ManaTypeData type)
        {
            var existingRecord = _cardContext.ManaTypes.FirstOrDefault(x => x.Id == type.Id);
            if (existingRecord == null)
            {
                _logger.LogWarning($"Adding mana type {type.Name}");
                _cardContext.ManaTypes.Add(type);
                await _cardContext.SaveChangesAsync();
            }
        }
        
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
        
        public async Task TryAddCardVariantType(CardVariantTypeData variant)
        {
            var existingRecord = _cardContext.VariantTypes.FirstOrDefault(x => x.Name == variant.Name);
            if (existingRecord == null)
            {
                _cardContext.VariantTypes.Add(variant);
                _logger.LogWarning($"Adding variant {variant.Name}");
                await _cardContext.SaveChangesAsync();
            }
            
        }
        
        public async Task TryAddDeckCardCategory(DeckCardCategoryData category)
        {
            var existingRecord = _cardContext.DeckCardCategories.FirstOrDefault(x => x.Id == category.Id);
            if (existingRecord == null)
            {
                _cardContext.DeckCardCategories.Add(category);
                _logger.LogWarning($"Adding category {category.Name}");
            }
            await _cardContext.SaveChangesAsync();
        }
    }
}
