using System;
using System.Collections.Generic;
using Carpentry.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Data.DataContext
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            //UriBuilder uri = new UriBuilder(codeBase);
            //string path = Uri.UnescapeDataString(uri.Path);
            //var baseDir = Path.GetDirectoryName(path) + "\\Migrations\\MovieActorsView.sql";

            //context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir));
            
        }

        private static void SeedFile(string fileName)
        {

        }
    }

    public class CarpentryDataContext : DbContext
    {
        #region DbSets

        //Tables
        public DbSet<CardLegalityData> CardLegalities { get; set; }
        public DbSet<CardData> Cards { get; set; }
        public DbSet<InventoryCardStatusData> CardStatuses { get; set; }
        public DbSet<DeckCardCategoryData> DeckCardCategories { get; set; }
        public DbSet<DeckCardData> DeckCards { get; set; }
        public DbSet<DeckData> Decks { get; set; }
        public DbSet<InventoryCardData> InventoryCards { get; set; } 
        public DbSet<MagicFormatData> MagicFormats { get; set; }
        public DbSet<CardRarityData> Rarities { get; set; }
        public DbSet<CardSetData> Sets { get; set; }
        public DbSet<DeckCardTagData> CardTags { get; set; }

        //Views
        public DbSet<DataModels.QueryResults.InventoryCardByNameResult> InventoryCardByName { get; set; }
        public DbSet<DataModels.QueryResults.InventoryCardByPrintResult> InventoryCardByPrint { get; set; }
        public DbSet<DataModels.QueryResults.InventoryCardByUniqueResult> InventoryCardByUnique { get; set; }
        public DbSet<DataModels.QueryResults.SetTotalsResult> SetTotals { get; set; }
        public DbSet<DataModels.QueryResults.TrimmingTipsResult> TrimmingTips { get; set; }
        public DbSet<DataModels.QueryResults.TrimmingTipsCountResult> TrimmingTipsTotalCount { get; set; }

        #endregion

        public CarpentryDataContext(DbContextOptions<CarpentryDataContext> options) : base(options)
        {
            //DB should be set on the configuration page
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Associations

            //Deck -- DeckCard
            modelBuilder.Entity<DeckData>()
                .HasMany(x => x.Cards)
                .WithOne(x => x.Deck)
                .HasForeignKey(x => x.DeckId);

            //DeckCard -- InventoryCard
            modelBuilder.Entity<DeckCardData>()
                .HasOne(x => x.InventoryCard)
                .WithMany(x => x.DeckCards)
                .HasForeignKey(x => x.InventoryCardId);

            //InventoryCard -- Card
            modelBuilder.Entity<InventoryCardData>()
                .HasOne(x => x.Card)
                .WithMany(x => x.InventoryCards)
                .HasForeignKey(x => x.CardId);

            //InventoryCard -- Status
            modelBuilder.Entity<InventoryCardData>()
                .HasOne(x => x.Status)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.InventoryCardStatusId);

            //Card -- Set
            modelBuilder.Entity<CardData>()
                .HasOne(x => x.Set)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.SetId);

            //Card -- Rarity
            modelBuilder.Entity<CardData>()
                .HasOne(x => x.Rarity)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.RarityId);

            //magic format -- card legality
            modelBuilder.Entity<CardLegalityData>()
                .HasOne(x => x.Format)
                .WithMany(x => x.LegalCards)
                .HasForeignKey(x => x.FormatId);

            //card -- card legality
            modelBuilder.Entity<CardLegalityData>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Legalities)
                .HasForeignKey(x => x.CardId);

            //deck card -- category
            modelBuilder.Entity<DeckCardData>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.CategoryId);

            //Deck -- MagicFormat
            modelBuilder.Entity<DeckData>()
                .HasOne(x => x.Format)
                .WithMany(x => x.Decks)
                .HasForeignKey(x => x.MagicFormatId);

            modelBuilder.Entity<DeckCardTagData>()
                .HasOne(x => x.Deck)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.DeckId);

            modelBuilder.Entity<DeckCardData>()
                .HasIndex(dc => dc.InventoryCardId)
                .IsUnique();

            #endregion

            #region Views

            modelBuilder.Entity<DataModels.QueryResults.InventoryCardByNameResult>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("vwInventoryCardsByName");
            });

            modelBuilder.Entity<DataModels.QueryResults.InventoryCardByPrintResult>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("vwInventoryCardsByPrint");
            });
            modelBuilder.Entity<DataModels.QueryResults.InventoryCardByUniqueResult>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("vwInventoryCardsByUnique");
            });
            modelBuilder.Entity<DataModels.QueryResults.InventoryTotalsByStatusResult>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("vwInventoryTotalsByStatus");
            });

            modelBuilder.Entity<DataModels.QueryResults.SetTotalsResult>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("vwSetTotals");
            });

            #endregion

            #region procs?

            modelBuilder.Entity<DataModels.QueryResults.TrimmingTipsResult>(eb => eb.HasNoKey());
            modelBuilder.Entity<DataModels.QueryResults.TrimmingTipsCountResult>(eb => eb.HasNoKey());

            #endregion

            modelBuilder.Seed();
        }

    }
}
