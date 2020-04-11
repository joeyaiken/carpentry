using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataContextLegacy
{
    public class LegacySqliteDataContext : DbContext
    {
        #region DbSets

        //Deck
        public DbSet<Deck> Decks { get; set; }

        ////DeckBuylistCard
        //public DbSet<DeckBuylistCard> DeckBuylistCards { get; set; }

        //DeckInventoryCard
        public DbSet<DeckCard> DeckCards { get; set; }

        ////BuylistItem / BuylistCard
        //public DbSet<BuylistCard> BuylistCards { get; set; }

        //InventoryCard / InventoryCard
        public DbSet<InventoryCard> InventoryCards { get; set; }

        //
        public DbSet<InventoryCardStatus> CardStatuses { get; set; }

        //Card
        public DbSet<Card> Cards { get; set; }

        //Card Set
        public DbSet<CardSet> Sets { get; set; }

        //Card Rarity
        public DbSet<CardRarity> Rarities { get; set; }

        //Mana Type
        public DbSet<ManaType> ManaTypes { get; set; }

        //Card Color Identities
        public DbSet<CardColorIdentity> ColorIdentities { get; set; }

        #endregion

        public LegacySqliteDataContext(DbContextOptions<LegacySqliteDataContext> options) : base(options)
        {
            //DB should be set on the configuration page
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Associations

            //so many associations........
            //modelBuilder.Entity<ScryfallSet>()
            //    //has key?
            //    .HasMany(x => x.Cards)
            //    .WithOne(x => x.Set)
            //    .HasForeignKey(x => x.ScryfallSetId);


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

            #endregion
        }
    }


    //Deck
    public class Deck
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public string Format { get; set; } //Should eventually be a FK table

        //Basic lands will still be tracked here (for now)
        public int BasicW { get; set; }

        public int BasicU { get; set; }

        public int BasicB { get; set; }

        public int BasicR { get; set; }

        public int BasicG { get; set; }

        //Associations

        //Deck -- DeckInventoryCard
        public List<DeckCard> Cards { get; set; }
    }

    //DeckCard
    public class DeckCard
    {
        public int Id { get; set; }

        public int DeckId { get; set; }

        public int InventoryCardId { get; set; }

        //Associations

        //DeckInventoryCard -- InventoryCard
        public InventoryCard InventoryCard { get; set; }

        //Deck -- DeckInventoryCard
        public Deck Deck { get; set; }

    }

    //InventoryCard / InventoryCard
    public class InventoryCard
    {
        public int Id { get; set; }

        public int MultiverseId { get; set; }

        public bool IsFoil { get; set; }

        //public int CardID { get; set; } //REMOVE THIS and reference by Multiverse ID ? 

        public int InventoryCardStatusId { get; set; }


        //Associations

        //InventoryCard -- Card
        public Card Card { get; set; }

        //DeckInventoryCard -- InventoryCard
        public List<DeckCard> DeckCards { get; set; }

        public InventoryCardStatus Status { get; set; }
    }

    public class InventoryCardStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<InventoryCard> Cards { get; set; }
    }

    //Card
    public class Card
    {
        public int Id { get; set; } //REMOVE THIS ? 

        //public int MultiverseId { get; set; } // OR JUST REMOVE THIS? 

        public int? Cmc { get; set; }

        //public List<string> ColorIdentity { get; set; }

        public string ImageUrl { get; set; }

        public string ImageArtCropUrl { get; set; }

        public string ManaCost { get; set; }

        public string Name { get; set; }

        public decimal? Price { get; set; }

        public decimal? PriceFoil { get; set; }

        //public string Rarity { get; set; }
        public int RarityId { get; set; }

        //public string Set { get; set; }
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

        //InventoryCard -- Card
        public List<InventoryCard> InventoryCards { get; set; }
    }

    //Card Set
    public class CardSet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        //Associations

        //Card -- Set
        public List<Card> Cards { get; set; }
    }

    //Card Rarity
    public class CardRarity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //Associations

        //Card -- Rarity
        public List<Card> Cards { get; set; }
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
    }

    //Card Color Identities
    public class CardColorIdentity
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

}
