using Carpentry.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Data
{
    public class CarpentryDataContext : DbContext
    {
        // Cards & related (stuff populated from scryfall or static data, not from backups)
        public DbSet<CardData> Cards { get; set; }
        // public DbSet<CardLegalityData> CardLegalities { get; set; }
        // public DbSet<InventoryCardStatusData> CardStatuses { get; set; }
        // public DbSet<MagicFormatData> MagicFormats { get; set; }
        // public DbSet<CardRarityData> Rarities { get; set; }
        // public DbSet<CardSetData> Sets { get; set; }
        
        // public DbSet<DeckCardCategoryData> DeckCardCategories { get; set; }
        
        // Things managed by backups
        // public DbSet<DeckCardData> DeckCards { get; set; }
        // public DbSet<DeckData> Decks { get; set; }
        // public DbSet<InventoryCardData> InventoryCards { get; set; }
        // public DbSet<DeckCardTagData> CardTags { get; set; }
        
    }
}