using System;
using System.Collections.Generic;
using Carpentry.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Data.DataContext
{
    public class CarpentryDataContext : DbContext
    {
        #region DbSets

        public DbSet<DeckData> Decks { get; set; }

        public DbSet<DeckCardData> DeckCards { get; set; }

        public DbSet<InventoryCardData> InventoryCards { get; set; }

        public DbSet<InventoryCardStatusData> CardStatuses { get; set; }

        public DbSet<CardData> Cards { get; set; }

        public DbSet<CardSetData> Sets { get; set; }

        public DbSet<CardRarityData> Rarities { get; set; }

        public DbSet<CardColorIdentityData> ColorIdentities { get; set; }

        public DbSet<CardColorData> CardColors { get; set; }

        public DbSet<ManaTypeData> ManaTypes { get; set; }

        public DbSet<CardLegalityData> CardLegalities { get; set; }

        public DbSet<MagicFormatData> MagicFormats { get; set; }

        public DbSet<CardVariantData> CardVariants { get; set; }

        public DbSet<CardVariantTypeData> VariantTypes { get; set; }

        public DbSet<DeckCardCategoryData> DeckCardCategories { get; set; }

        #endregion

        public CarpentryDataContext(DbContextOptions<CarpentryDataContext> options) : base(options)
        {
            //DB should be set on the configuration page
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Associations

            //Deck -- DeckInventoryCard
            modelBuilder.Entity<DeckData>()
                .HasMany(x => x.Cards)
                .WithOne(x => x.Deck)
                .HasForeignKey(x => x.DeckId);

            //DeckInventoryCard -- InventoryCard
            modelBuilder.Entity<DeckCardData>()
                .HasOne(x => x.InventoryCard)
                .WithMany(x => x.DeckCards)
                .HasForeignKey(x => x.InventoryCardId);

            //InventoryCard -- Card
            modelBuilder.Entity<InventoryCardData>()
                .HasOne(x => x.Card)
                .WithMany(x => x.InventoryCards)
                .HasForeignKey(x => x.MultiverseId);

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

            //Card -- CardColorIdentity
            modelBuilder.Entity<CardData>()
                .HasMany(x => x.CardColorIdentities)
                .WithOne(x => x.Card)
                .HasForeignKey(x => x.CardId);

            //CardColorIdentity -- ManaType
            modelBuilder.Entity<ManaTypeData>()
                .HasMany(x => x.CardColorIdentities)
                .WithOne(x => x.ManaType)
                .HasForeignKey(x => x.ManaTypeId);

            //variant type -- card variant
            modelBuilder.Entity<CardVariantData>()
                .HasOne(x => x.Type)
                .WithMany(x => x.VariantCards)
                .HasForeignKey(x => x.CardVariantTypeId);

            //card -- card variant
            modelBuilder.Entity<CardVariantData>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Variants)
                .HasForeignKey(x => x.CardId);

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

            //card color -- mana type
            modelBuilder.Entity<ManaTypeData>()
                .HasMany(x => x.CardColors)
                .WithOne(x => x.ManaType)
                .HasForeignKey(x => x.ManaTypeId);

            //card -- card color
            modelBuilder.Entity<CardData>()
                .HasMany(x => x.CardColors)
                .WithOne(x => x.Card)
                .HasForeignKey(x => x.CardId);

            //deck card -- category
            modelBuilder.Entity<DeckCardData>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.CategoryId);

            ////inventory card - variant type
            //modelBuilder.Entity<InventoryCard>()
            //    .HasOne(x => x.VariantType)
            //    .WithMany(x => x.InventoryCards)
            //    .HasForeignKey(x => x.VariantTypeId);

            //Deck -- MagicFormat
            modelBuilder.Entity<DeckData>()
                .HasOne(x => x.Format)
                .WithMany(x => x.Decks)
                .HasForeignKey(x => x.MagicFormatId);

            #endregion
        }

    }
}
