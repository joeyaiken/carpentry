using System.Collections.Generic;
using Carpentry.CarpentryData.Models;

namespace Carpentry.CarpentryData
{
    public class ReferenceValues
    {
        public ReferenceValues()
        {
            StandardFormat = new MagicFormatData { Name = "standard" };
            
            Formats = new List<MagicFormatData>()
            {
                // new MagicFormatData { Name = "standard" },
                StandardFormat,
                //new MagicFormat { Name = "future" },
                //new MagicFormat { Name = "historic" },
                new MagicFormatData { Name = "pioneer" },
                new MagicFormatData { Name = "modern" },
                //new MagicFormat { Name = "legacy" },
                new MagicFormatData { Name = "pauper" },
                //new MagicFormat { Name = "vintage" },
                //new MagicFormat { Name = "penny" },
                new MagicFormatData { Name = "commander" },
                new MagicFormatData { Name = "brawl" },
                new MagicFormatData { Name = "jumpstart" },
                new MagicFormatData { Name = "sealed" },
                //new MagicFormat { Name = "duel" },
                //new MagicFormat { Name = "oldschool" },
            };
            
            CardStatuses = new List<InventoryCardStatusData>()
            {
                new InventoryCardStatusData { CardStatusId = 1, Name = "Inventory" },
                new InventoryCardStatusData { CardStatusId = 2, Name = "Buy List" },
                new InventoryCardStatusData { CardStatusId = 3, Name = "Sell List" },
            };
            
            CardRarities = new List<CardRarityData>()
            {
                new CardRarityData
                {
                    RarityId = 'M',
                    Name = "mythic",
                },
                new CardRarityData
                {
                    RarityId = 'R',
                    Name = "rare",
                },
                new CardRarityData
                {
                    RarityId = 'U',
                    Name = "uncommon",
                },
                new CardRarityData
                {
                    RarityId = 'C',
                    Name = "common",
                },
                new CardRarityData
                {
                    RarityId = 'S',
                    Name = "special",
                },
            };

            CardCategories = new List<DeckCardCategoryData>()
            {
                //null == mainboard new DeckCardCategory { Id = '', Name = "" },
                new DeckCardCategoryData { DeckCardCategoryId = 'c', Name = "Commander" },
                new DeckCardCategoryData { DeckCardCategoryId = 's', Name = "Sideboard" },
                //Companion

                //new DeckCardCategory { Id = '', Name = "" },
                //new DeckCardCategory { Id = '', Name = "" },
            };
        }
        
        public MagicFormatData StandardFormat { get; }
        
        public List<MagicFormatData> Formats { get; }
        
        public List<InventoryCardStatusData> CardStatuses { get; }
        
        public List<CardRarityData> CardRarities { get; }
        
        public List<DeckCardCategoryData> CardCategories { get; }

        public List<object> AllEntities()
        {
            
            var result = new List<object>();
            result.AddRange(Formats);
            result.AddRange(CardStatuses);
            result.AddRange(CardRarities);
            result.AddRange(CardCategories);
            return result;
        }
    }
}