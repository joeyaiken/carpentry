using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Carpentry.ScryfallData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    public static class SeedData
    {
        public static DeckData Deck1 = new DeckData()
        {
            BasicW = 5,
            BasicR = 5,
            Name = "testDeck",
            Cards = new List<DeckCardData>()
                {
                    new DeckCardData()
                    {

                        InventoryCard = new InventoryCardData()
                        {

                        },
                    },
                    new DeckCardData(){ },
                    new DeckCardData(){ },
                },

        };



        public static CardSetData TestSet = new CardSetData()
        {


            Cards = new List<CardData>()
            {
                //Creature G
                new CardData()
                {
                    CardId = 0,
                    Cmc = 0,
                    ManaCost = "",
                    Name = "",
                    RarityId = 'R',
                    Text = "",
                    Type = "",
                    MultiverseId = 0,
                    Price = 0,
                    PriceFoil = 0,
                    ImageUrl = "",
                    CollectorNumber = 0,
                    TixPrice = 0,
                    Color = "",
                    ColorIdentity = "",
                },
                //Instant R
                new CardData()
                {
                    CardId = 0,
                    Cmc = 0,
                    ManaCost = "",
                    Name = "",
                    RarityId = 'R',
                    Text = "",
                    Type = "",
                    MultiverseId = 0,
                    Price = 0,
                    PriceFoil = 0,
                    ImageUrl = "",
                    CollectorNumber = 0,
                    TixPrice = 0,
                    Color = "",
                    ColorIdentity = "",
                },
                //Sorcery U
                new CardData()
                {
                    CardId = 0,
                    Cmc = 0,
                    ManaCost = "",
                    Name = "",
                    RarityId = 'R',
                    Text = "",
                    Type = "",
                    MultiverseId = 0,
                    Price = 0,
                    PriceFoil = 0,
                    ImageUrl = "",
                    CollectorNumber = 0,
                    TixPrice = 0,
                    Color = "",
                    ColorIdentity = "",
                },                
                //Enchant W
                new CardData()
                {
                    CardId = 0,
                    Cmc = 0,
                    ManaCost = "",
                    Name = "",
                    RarityId = 'R',
                    Text = "",
                    Type = "",
                    MultiverseId = 0,
                    Price = 0,
                    PriceFoil = 0,
                    ImageUrl = "",
                    CollectorNumber = 0,
                    TixPrice = 0,
                    Color = "",
                    ColorIdentity = "",
                },
                //Planeswalker B
                new CardData()
                {
                    CardId = 0,
                    Cmc = 0,
                    ManaCost = "",
                    Name = "",
                    RarityId = 'R',
                    Text = "",
                    Type = "",
                    MultiverseId = 0,
                    Price = 0,
                    PriceFoil = 0,
                    ImageUrl = "",
                    CollectorNumber = 0,
                    TixPrice = 0,
                    Color = "",
                    ColorIdentity = "",
                },

                //Artifact []
                new CardData()
                {
                    CardId = 0,
                    Cmc = 0,
                    ManaCost = "",
                    Name = "",
                    RarityId = 'R',
                    Text = "",
                    Type = "",
                    MultiverseId = 0,
                    Price = 0,
                    PriceFoil = 0,
                    ImageUrl = "",
                    CollectorNumber = 0,
                    TixPrice = 0,
                    Color = "",
                    ColorIdentity = "",
                },
                //Land ??
                //new CardData()
                //{
                //    CardId = 0,
                //    Cmc = 0,
                //    ManaCost = "",
                //    Name = "",
                //    RarityId = 'R',
                //    Text = "",
                //    Type = "",
                //    MultiverseId = 0,
                //    Price = 0,
                //    PriceFoil = 0,
                //    ImageUrl = "",
                //    CollectorNumber = 0,
                //    TixPrice = 0,
                //    Color = "",
                //    ColorIdentity = "",
                //},


            },

        };
    }

    //Consider renaming to 'CarpentryDataTestBase' or something?
    public abstract class CarpentryServiceTestBase
    {

        protected CarpentryDataContext CardContext = null!;
        protected ScryfallDataContext ScryContext = null!;

        private DbContextOptions<CarpentryDataContext> _cardContextOptions = null!;
        private DbContextOptions<ScryfallDataContext> _scryContextOptions = null!;

        protected abstract bool SeedViews { get; }

        [TestInitialize]
        public async Task BeforeEach()
        {
            _cardContextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
                .UseSqlite("Filename=CarpentryData.db").Options;
            _scryContextOptions = new DbContextOptionsBuilder<ScryfallDataContext>()
                .UseSqlite("Filename=ScryData.db").Options;

            ResetContext();
            
            await CardContext.EnsureDatabaseCreated(false);
            await ScryContext.Database.EnsureCreatedAsync();

            ResetContext();

            await BeforeEachChild();
        }

        protected abstract Task BeforeEachChild();

        [TestCleanup]
        public async Task AfterEach()
        {
            await CardContext.Database.EnsureDeletedAsync();
            await CardContext.DisposeAsync();

            await ScryContext.Database.EnsureDeletedAsync();
            await ScryContext.DisposeAsync();

            await AfterEachChild();
        }

        protected abstract Task AfterEachChild();

        protected void ResetContext()
        {
            var mockDbLogger = new Mock<ILogger<CarpentryDataContext>>();
            CardContext = new CarpentryDataContext(_cardContextOptions, mockDbLogger.Object);
            ScryContext = new ScryfallDataContext(_scryContextOptions);
            ResetContextChild();
        }

        protected abstract void ResetContextChild();
    }
}
