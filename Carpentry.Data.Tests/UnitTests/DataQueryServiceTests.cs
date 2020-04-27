using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.Implementations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
//using System;
using System.Collections.Generic;
//using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Carpentry.Data.Tests.UnitTests
{
    [TestClass]
    public class DataQueryServiceTests
    {

        //Task<IEnumerable<CardOverviewResult>> GetInventoryOverviews(InventoryQueryParameter param);
        [TestMethod]
        public async Task DataQueryService_GetInventoryOverviews_Test()
        {
            //Arrange


            //Act


            //Assert
            Assert.Fail();
        }

        //Task<IEnumerable<CardOverviewResult>> GetDeckCardOverviews(int deckId);
        [TestMethod]
        public async Task DataQueryService_GetDeckCardOverviews_Test()
        {
            //Want to ensure an in-memory DB is used
            var connection = new SqliteConnection("DataSource=:memory:");

            connection.Open();

            try
            {
                //Arrange
                var contextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
                    .UseSqlite(connection)
                    .Options;

                DeckData testDeck = new DeckData()
                {
                    Name = "Test Deck",
                    Format = new MagicFormatData()
                    {
                        Name = "Modern",
                    },
                    
                };

                InventoryCardStatusData testStatus = new InventoryCardStatusData()
                {
                    Name = "Test Status",
                };

                CardVariantTypeData testVariantType = new CardVariantTypeData()
                {
                    Name = "Test Variant Type",
                };

                CardSetData testSet = new CardSetData()
                {
                    Name = "Test Set",
                    Code = "TST",
                };

                CardRarityData testRarity = new CardRarityData()
                {
                    Id = 'T',
                    Name = "Test",
                };

                List<DeckCardData> deckCards = new List<DeckCardData>()
                {
                    //Card 1
                    new DeckCardData
                    {
                        InventoryCard = new InventoryCardData()
                        {
                            Card = new CardData()
                            {
                                Name = "Card 1",
                                Rarity = testRarity,
                                Set = testSet,
                            },
                            Status = testStatus,
                            IsFoil = false,
                            VariantType = testVariantType,
                        },
                        Deck = testDeck,
                        CategoryId = null,
                    },
                    //Card 2
                    new DeckCardData
                    {
                        InventoryCard = new InventoryCardData()
                        {
                            Card = new CardData()
                            {
                                Name = "Card 2",
                                Rarity = testRarity,
                                Set = testSet,
                            },
                            Status = testStatus,
                            IsFoil = false,
                            VariantType = testVariantType,
                        },
                        Deck = testDeck,
                        CategoryId = null,
                    },
                    //Card 3
                    new DeckCardData
                    {
                        InventoryCard = new InventoryCardData()
                        {
                            Card = new CardData()
                            {
                                Name = "Card 3",
                                Rarity = testRarity,
                                Set = testSet,
                            },
                            Status = testStatus,
                            IsFoil = false,
                            VariantType = testVariantType,
                        },
                        Deck = testDeck,
                        CategoryId = null,
                    },
                    //Card 4
                    new DeckCardData
                    {
                        InventoryCard = new InventoryCardData()
                        {
                            Card = new CardData()
                            {
                                Name = "Card 4",
                                Rarity = testRarity,
                                Set = testSet,
                            },
                            Status = testStatus,
                            IsFoil = false,
                            VariantType = testVariantType,
                        },
                        Deck = testDeck,
                        CategoryId = null,
                    },
                };


                using (var context = new CarpentryDataContext(contextOptions))
                {
                    context.Database.EnsureCreated();

                    //add default records
                    context.Decks.Add(testDeck);
                    context.CardStatuses.Add(testStatus);
                    context.VariantTypes.Add(testVariantType);
                    context.Sets.Add(testSet);
                    context.Rarities.Add(testRarity);
                    context.DeckCards.AddRange(deckCards);
                    context.SaveChanges();



                }

                int deckIdToRequest = 1;

                List<QueryResults.CardOverviewResult> serviceResult = null;

                //Act
                using (var context = new CarpentryDataContext(contextOptions))
                {
                    var mockLogger = new Mock<ILogger<DataQueryService>>(MockBehavior.Loose);

                    var queryService = new DataQueryService(context, mockLogger.Object);

                    var result = await queryService.GetDeckCardOverviews(deckIdToRequest);
                    
                    serviceResult = result.ToList();
                }

                //Assert

                Assert.AreEqual(4, serviceResult.Count());

                //var dbContext = new CarpentryDataContext(contextOptions);

                //var mockLogger = new Mock<ILogger<DataQueryService>>(MockBehavior.Loose);

                //var queryService = new DataQueryService(dbContext, mockLogger.Object);


            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception thrown: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        //Task<int> GetDeckCardCount(int deckId);
        [TestMethod]
        public async Task DataQueryService_GetDeckCardCount_Test()
        {
            //Arrange


            //Act


            //Assert
            Assert.Fail();
        }

        //Task<IEnumerable<InventoryCardResult>> GetDeckInventoryCards(int deckId);
        [TestMethod]
        public async Task DataQueryService_GetDeckInventoryCards_Test()
        {
            //Arrange

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                //Arrange
                var contextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
                    .UseSqlite(connection)
                    .Options;

                #region stage data

                DeckData testDeck = new DeckData()
                {
                    Name = "Test Deck",
                    Format = new MagicFormatData()
                    {
                        Name = "Modern",
                    },

                };

                InventoryCardStatusData testStatus = new InventoryCardStatusData()
                {
                    Name = "Test Status",
                };

                CardVariantTypeData testVariantType = new CardVariantTypeData()
                {
                    Name = "Test Variant Type",
                };

                CardSetData testSet = new CardSetData()
                {
                    Name = "Test Set",
                    Code = "TST",
                };

                CardRarityData testRarity = new CardRarityData()
                {
                    Id = 'T',
                    Name = "Test",
                };

                List<DeckCardData> deckCards = new List<DeckCardData>()
                {
                    //Card 1
                    new DeckCardData
                    {
                        InventoryCard = new InventoryCardData()
                        {
                            Card = new CardData()
                            {
                                Name = "Card 1",
                                Rarity = testRarity,
                                Set = testSet,
                            },
                            Status = testStatus,
                            IsFoil = false,
                            VariantType = testVariantType,
                        },
                        Deck = testDeck,
                        CategoryId = null,
                    },
                    //Card 2
                    new DeckCardData
                    {
                        InventoryCard = new InventoryCardData()
                        {
                            Card = new CardData()
                            {
                                Name = "Card 2",
                                Rarity = testRarity,
                                Set = testSet,
                            },
                            Status = testStatus,
                            IsFoil = false,
                            VariantType = testVariantType,
                        },
                        Deck = testDeck,
                        CategoryId = null,
                    },
                    //Card 3
                    new DeckCardData
                    {
                        InventoryCard = new InventoryCardData()
                        {
                            Card = new CardData()
                            {
                                Name = "Card 3",
                                Rarity = testRarity,
                                Set = testSet,
                            },
                            Status = testStatus,
                            IsFoil = false,
                            VariantType = testVariantType,
                        },
                        Deck = testDeck,
                        CategoryId = null,
                    },
                    //Card 4
                    new DeckCardData
                    {
                        InventoryCard = new InventoryCardData()
                        {
                            Card = new CardData()
                            {
                                Name = "Card 4",
                                Rarity = testRarity,
                                Set = testSet,
                            },
                            Status = testStatus,
                            IsFoil = false,
                            VariantType = testVariantType,
                        },
                        Deck = testDeck,
                        CategoryId = null,
                    },
                };

                #endregion stage data

                using (var context = new CarpentryDataContext(contextOptions))
                {
                    context.Database.EnsureCreated();

                    //add default records
                    context.Decks.Add(testDeck);
                    context.CardStatuses.Add(testStatus);
                    context.VariantTypes.Add(testVariantType);
                    context.Sets.Add(testSet);
                    context.Rarities.Add(testRarity);
                    context.DeckCards.AddRange(deckCards);
                    context.SaveChanges();



                }

                int deckIdToRequest = 1;

                List<QueryResults.InventoryCardResult> serviceResult = null;

                //Act
                using (var context = new CarpentryDataContext(contextOptions))
                {
                    var mockLogger = new Mock<ILogger<DataQueryService>>(MockBehavior.Loose);

                    var queryService = new DataQueryService(context, mockLogger.Object);

                    var result = await queryService.GetDeckInventoryCards(deckIdToRequest);

                    serviceResult = result.ToList();
                }

                //Assert

                Assert.AreEqual(4, serviceResult.Count());

                //var dbContext = new CarpentryDataContext(contextOptions);

                //var mockLogger = new Mock<ILogger<DataQueryService>>(MockBehavior.Loose);

                //var queryService = new DataQueryService(dbContext, mockLogger.Object);


            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception thrown: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        //Task<IEnumerable<DeckCardStatResult>> GetDeckCardStats(int deckId);
        [TestMethod]
        public async Task DataQueryService_GetDeckCardStats_Test()
        {
            //Arrange


            //Act


            //Assert
            Assert.Fail();
        }

        //Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName);
        [TestMethod]
        public async Task DataQueryService_GetInventoryCardsByName_Test()
        {
            //Arrange


            //Act


            //Assert
            Assert.Fail();
        }

        //Task<IEnumerable<CardDataDto>> SearchInventoryCards(InventoryQueryParameter filters);
        [TestMethod]
        public async Task DataQueryService_SearchInventoryCards_Test()
        {
            //Arrange


            //Act


            //Assert
            Assert.Fail();
        }

        //Task<IEnumerable<CardDataDto>> SearchCardSet(CardSearchQueryParameter filters);
        [TestMethod]
        public async Task DataQueryService_SearchCardSet_Test()
        {
            //Arrange


            //Act


            //Assert
            Assert.Fail();
        }

    }
}
