using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Carpentry.ScryfallData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    public class SeedData
    {
        public readonly ReferenceValues ReferenceValues;

        public SeedData(ReferenceValues referenceValues)
        {
            ReferenceValues = referenceValues;
            
            TripleThreatDeck = new DeckData()
            {
                Name = "Triple Threat",
                BasicU = 7,
                // MagicFormatId = referenceValues.Formats.First().FormatId,
                // Format = referenceValues.Formats.First(),
                // Format = referenceValues.StandardFormat,
                MagicFormatId = 1,
                Cards = new List<DeckCardData>()
                {
                    new DeckCardData()
                    {
                        CardName = "Storm Crow"
                    },
                    new DeckCardData()
                    {
                        CardName = "Storm Crow"
                    },
                    new DeckCardData()
                    {
                        CardName = "Storm Crow"
                    },
                }
            };
            
            StormCrowSet = new CardSetData()
            {
                Name = "Storm's Crow",
                Code = "STC",
                Cards = new List<CardData>()
                {
                    //Storm Crow
                    new CardData()
                    {
                        Name = "Storm Crow",
                        RarityId = 'U',
                        ImageUrl = "https://c1.scryfall.com/file/scryfall-cards/normal/front/1/3/13f6106d-6822-47b5-956e-20e17143a818.jpg?1562898988",
                        Type = "Creature",
                        ColorIdentity = "U",
                    },
                    // //Not Storm Crow
                    // new CardData(){ },
                    // //
                    // new CardData(){ },
                }
            };
            
            
        }
        
        public DeckData TripleThreatDeck { get; }
        public CardSetData StormCrowSet { get; }
    }
    
    public static class StaticSeedData
    {
        
        //A test deck with multiple named cards
        //Those deck cards won't need an inventory card, but they'll need card definitions with the same name
        public static DeckData Deck1 = new DeckData()
        {
            BasicW = 5,
            BasicR = 5,
            Name = "testDeck",
            Cards = new List<DeckCardData>()
                {
                    new DeckCardData()
                    {
                        CardName = "Storm Crow"
                        
                        // InventoryCard = new InventoryCardData()
                        // {
                        //
                        // },
                    },
                    new DeckCardData(){ },
                    new DeckCardData(){ },
                },

        };

        // public static DeckData TripleThreatDeck = new DeckData()
        // {
        //     Name = "Triple Threat",
        //     BasicU = 7,
        //     // Cards = new List<DeckCardData>()
        //     // {
        //     //     new DeckCardData()
        //     //     {
        //     //         CardName = "Storm Crow"
        //     //     },
        //     //     new DeckCardData()
        //     //     {
        //     //         CardName = "Storm Crow"
        //     //     },
        //     //     new DeckCardData()
        //     //     {
        //     //         CardName = "Storm Crow"
        //     //     },
        //     // }
        // };
        //
        // public static CardSetData StormCrowSet = new CardSetData()
        // {
        //     Name = "Storm's Crow",
        //     Code = "STC",
        //     Cards = new List<CardData>()
        //     {
        //         //Storm Crow
        //         new CardData()
        //         {
        //             Name = "Storm Crow",
        //             RarityId = 'U',
        //             ImageUrl = "https://c1.scryfall.com/file/scryfall-cards/normal/front/1/3/13f6106d-6822-47b5-956e-20e17143a818.jpg?1562898988"
        //         },
        //         // //Not Storm Crow
        //         // new CardData(){ },
        //         // //
        //         // new CardData(){ },
        //     }
        // };
        //


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

        // protected ReferenceValues ReferenceValues;
        protected SeedData SeedData;

        private DbContextOptions<CarpentryDataContext> _cardContextOptions = null!;
        private DbContextOptions<ScryfallDataContext> _scryContextOptions = null!;

        protected abstract bool SeedViews { get; }

        //[TestInitialize]
        //public async Task BeforeEach()
        //{
        //    _cardContextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
        //        .UseSqlite("Filename=CarpentryData.db").Options;
        //    _scryContextOptions = new DbContextOptionsBuilder<ScryfallDataContext>()
        //        .UseSqlite("Filename=ScryData.db").Options;

        //    ResetContext();
            
        //    await CardContext.EnsureDatabaseCreated(false);
        //    await ScryContext.Database.EnsureCreatedAsync();

        //    ResetContext();

        //    await BeforeEachChild();
        //}

        protected async Task BeforeEachBase()
        {
            _cardContextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
                .UseSqlite("Filename=CarpentryData.db").Options;
            _scryContextOptions = new DbContextOptionsBuilder<ScryfallDataContext>()
                .UseSqlite("Filename=ScryData.db").Options;

            ResetContext();
            //Yeah I shouldn't NEED this, but I first need to make these tests better
            await CardContext.Database.EnsureDeletedAsync();
            await ScryContext.Database.EnsureDeletedAsync();
            // await CardContext.EnsureDatabaseCreated(false);
            await CardContext.Database.EnsureCreatedAsync();
            await ScryContext.Database.EnsureCreatedAsync();

            var referenceValues = new ReferenceValues();
            CardContext.AddRange(referenceValues.AllEntities());
            
            SeedData = new SeedData(referenceValues);
            
            // ReferenceValues = new ReferenceValues();
            // ReSharper disable once MethodHasAsyncOverload
            await CardContext.SaveChangesAsync();
            ResetContext();
        }

        //protected abstract Task BeforeEachChild();

        //[TestCleanup]
        //public async Task AfterEach()
        //{
        //    await CardContext.Database.EnsureDeletedAsync();
        //    await CardContext.DisposeAsync();

        //    await ScryContext.Database.EnsureDeletedAsync();
        //    await ScryContext.DisposeAsync();

        //    await AfterEachChild();
        //}

        protected async Task AfterEachBase()
        {
            await CardContext.Database.EnsureDeletedAsync();
            await CardContext.DisposeAsync();

            await ScryContext.Database.EnsureDeletedAsync();
            await ScryContext.DisposeAsync();
        }

        //protected abstract Task AfterEachChild();

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
