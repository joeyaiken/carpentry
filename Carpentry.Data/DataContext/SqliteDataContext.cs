using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Data.DataContext
{
    public class SqliteDataContext : DbContext
    {
        #region DbSets

        public DbSet<Deck> Decks { get; set; }

        public DbSet<DeckCard> DeckCards { get; set; }

        public DbSet<InventoryCard> InventoryCards { get; set; }

        public DbSet<InventoryCardStatus> CardStatuses { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<CardSet> Sets { get; set; }

        public DbSet<CardRarity> Rarities { get; set; }

        public DbSet<CardColorIdentity> ColorIdentities { get; set; }

        public DbSet<CardColor> CardColors { get; set; }

        public DbSet<ManaType> ManaTypes { get; set; }

        public DbSet<CardLegality> CardLegalities { get; set; }

        public DbSet<MagicFormat> MagicFormats { get; set; }

        public DbSet<CardVariant> CardVariants { get; set; }

        public DbSet<CardVariantType> VariantTypes { get; set; }

        public DbSet<DeckCardCategory> DeckCardCategories { get; set; }

        #endregion

        public SqliteDataContext(DbContextOptions<SqliteDataContext> options) : base(options)
        {
            //DB should be set on the configuration page
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Associations

            //Deck -- DeckInventoryCard
            modelBuilder.Entity<Deck>()
                .HasMany(x => x.Cards)
                .WithOne(x => x.Deck)
                .HasForeignKey(x => x.DeckId);

            //DeckInventoryCard -- InventoryCard
            modelBuilder.Entity<DeckCard>()
                .HasOne(x => x.InventoryCard)
                .WithMany(x => x.DeckCards)
                .HasForeignKey(x => x.InventoryCardId);

            //InventoryCard -- Card
            modelBuilder.Entity<InventoryCard>()
                .HasOne(x => x.Card)
                .WithMany(x => x.InventoryCards)
                .HasForeignKey(x => x.MultiverseId);

            //InventoryCard -- Status
            modelBuilder.Entity<InventoryCard>()
                .HasOne(x => x.Status)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.InventoryCardStatusId);

            //Card -- Set
            modelBuilder.Entity<Card>()
                .HasOne(x => x.Set)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.SetId);

            //Card -- Rarity
            modelBuilder.Entity<Card>()
                .HasOne(x => x.Rarity)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.RarityId);

            //Card -- CardColorIdentity
            modelBuilder.Entity<Card>()
                .HasMany(x => x.CardColorIdentities)
                .WithOne(x => x.Card)
                .HasForeignKey(x => x.CardId);

            //CardColorIdentity -- ManaType
            modelBuilder.Entity<ManaType>()
                .HasMany(x => x.CardColorIdentities)
                .WithOne(x => x.ManaType)
                .HasForeignKey(x => x.ManaTypeId);

            //variant type -- card variant
            modelBuilder.Entity<CardVariant>()
                .HasOne(x => x.Type)
                .WithMany(x => x.VariantCards)
                .HasForeignKey(x => x.CardVariantTypeId);

            //card -- card variant
            modelBuilder.Entity<CardVariant>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Variants)
                .HasForeignKey(x => x.CardId);

            //magic format -- card legality
            modelBuilder.Entity<CardLegality>()
                .HasOne(x => x.Format)
                .WithMany(x => x.LegalCards)
                .HasForeignKey(x => x.FormatId);

            //card -- card legality
            modelBuilder.Entity<CardLegality>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Legalities)
                .HasForeignKey(x => x.CardId);

            //card color -- mana type
            modelBuilder.Entity<ManaType>()
                .HasMany(x => x.CardColors)
                .WithOne(x => x.ManaType)
                .HasForeignKey(x => x.ManaTypeId);

            //card -- card color
            modelBuilder.Entity<Card>()
                .HasMany(x => x.CardColors)
                .WithOne(x => x.Card)
                .HasForeignKey(x => x.CardId);

            //deck card -- category
            modelBuilder.Entity<DeckCard>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.CategoryId);

            ////inventory card - variant type
            //modelBuilder.Entity<InventoryCard>()
            //    .HasOne(x => x.VariantType)
            //    .WithMany(x => x.InventoryCards)
            //    .HasForeignKey(x => x.VariantTypeId);

            //Deck -- MagicFormat
            modelBuilder.Entity<Deck>()
                .HasOne(x => x.Format)
                .WithMany(x => x.Decks)
                .HasForeignKey(x => x.MagicFormatId);

            #endregion
        }

    }
    
    public class Deck
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        //public string Format { get; set; } //Should eventually be a FK table
        public int MagicFormatId { get; set; }

        //Basic lands will still be tracked here (for now)
        public int BasicW { get; set; }

        public int BasicU { get; set; }

        public int BasicB { get; set; }

        public int BasicR { get; set; }

        public int BasicG { get; set; }

        //Associations

        //Deck -- DeckInventoryCard
        public List<DeckCard> Cards { get; set; }

        //Deck -- MagicFormat
        public MagicFormat Format { get; set; }
    }

    public class DeckCard
    {
        public int Id { get; set; }

        public int DeckId { get; set; }

        public int InventoryCardId { get; set; }
        
        public char? CategoryId { get; set; }

        //Associations

        //DeckInventoryCard -- InventoryCard
        public InventoryCard InventoryCard { get; set; }

        //Deck -- DeckInventoryCard
        public Deck Deck { get; set; }
        //DeckCard - Status
        public DeckCardCategory Category { get; set; }

    }

    public class InventoryCard
    {
        //Fields
        public int Id { get; set; }

        public int MultiverseId { get; set; }

        public int InventoryCardStatusId { get; set; }

        public char VariantTypeId { get; set; }


        //is 'foil' a variant type, or an inventory card property?
        //I think it should be an inventory card property
        public bool IsFoil { get; set; }

        //Associations
        public Card Card { get; set; }
        
        public List<DeckCard> DeckCards { get; set; }
        
        public InventoryCardStatus Status { get; set; }
        
        public CardVariantType VariantType { get; set; }
    }

    public class InventoryCardStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //Associations
        public List<InventoryCard> Cards { get; set; }
    }

    public class Card
    {
        public int Id { get; set; }

        public int? Cmc { get; set; }

        //public string ImageUrl { get; set; }

        //public string ImageArtCropUrl { get; set; }

        public string ManaCost { get; set; }

        public string Name { get; set; }

        //public decimal? Price { get; set; }

        //public decimal? PriceFoil { get; set; }

        public char RarityId { get; set; }

        public int SetId { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }

        //Associations

        //Card -- Set
        public CardSet Set { get; set; }

        //Card -- Rarity
        public CardRarity Rarity { get; set; }

        //Card -- CardColorIdentity
        public List<CardColorIdentity> CardColorIdentities { get; set; }

        public List<CardColor> CardColors { get; set; }

        //InventoryCard -- Card
        public List<InventoryCard> InventoryCards { get; set; }

        //variant
        public List<CardVariant> Variants { get; set; }

        //legal sets
        public List<CardLegality> Legalities { get; set; }
    }

    //Card Set
    public class CardSet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime ReleaseDate { get; set; }

        //Associations

        //Card -- Set
        public List<Card> Cards { get; set; }
    }

    //Card Rarity
    public class CardRarity
    {
        //char or int ??
        //public int Id { get; set; }
        public char Id { get; set; }

        public string Name { get; set; }

        //Associations

        //Card -- Rarity
        public List<Card> Cards { get; set; }
    }

    //Card Color Identities
    public class CardColorIdentity : CardColorPair
    {

    }

    //Card Colors
    public class CardColor : CardColorPair
    {
        

    }

    public class CardColorPair
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public char ManaTypeId { get; set; }

        //Associations

        //CardColorIdentity -- ManaType
        public ManaType ManaType { get; set; }

        //Card -- CardColorIdentity
        public Card Card { get; set; }

    }

    //Mana Type
    public class ManaType
    {
        //public int Id { get; set; }
        public char Id { get; set; }
        public string Name { get; set; }
        //public char Abbreviation { get; set; }

        //Associations

        //CardColorIdentity -- ManaType
        public List<CardColorIdentity> CardColorIdentities { get; set; }

        //cards whose colors contain this mana type
        public List<CardColor> CardColors { get; set; }
    }

    public class DeckCardCategory
    {
        //ID
        public char Id { get; set; }

        public string Name { get; set; }

        //Associations
        public List<DeckCard> Cards { get; set; }
    }

    public class MagicFormat
    {
        //id
        public int Id { get; set; }

        //name
        public string Name { get; set; }

        //Associations
        //card legalities
        public List<CardLegality> LegalCards { get; set; }
        public List<Deck> Decks { get; set; }
    }

    public class CardLegality
    {
        //id
        public int Id { get; set; }

        //card id
        public int CardId { get; set; }

        //format id
        public int FormatId { get; set; }

        //Associations
        public Card Card { get; set; }
        public MagicFormat Format { get; set; }
    }

    //This is still partially wrong
    //A Card has several variant types, those options are stored in the "Cadr Variants" table
    //An inventory card is an instance of a card of a specific variant type
    public class CardVariant
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public char CardVariantTypeId { get; set; }

        public decimal? Price { get; set; }
        public decimal? PriceFoil { get; set; }
        
        public string ImageUrl { get; set; }
        
        //associations
        public Card Card { get; set; }
        public CardVariantType Type { get; set; }
    }

    //THIS needs an association to all inventory items of the variant type
    //The actual inventory card will have a FK referencing a specific Card Variant
    //(what if variant type was a CHAR and not an INT??)
    //NOTE: FOIL is no longer a variant type
    //  Does this allow for CHAR IDs ?
    /*
        N   N - Normal
        B   BL - Borderless (plainswalker)
        E   EA - Extended Art (non-PW)
        S   SC - Showcase (storybook)
        F   FP - FatPack alternative art promo
        
    */
    public class CardVariantType
    {
        //id
        //Should ID be a char?
        //why not
        public char Id { get; set; }

        //name
        public string Name { get; set; }

        //associations
        //cards of this variant
        //do I really need an association in this direction?
        //'all cards of a given variant'
        public List<CardVariant> VariantCards { get; set; }
        public List<InventoryCard> InventoryCards { get; set; }
    }
}
