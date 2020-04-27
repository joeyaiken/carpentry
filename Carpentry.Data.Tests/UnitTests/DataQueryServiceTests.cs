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
            //Arrange

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                //Arrange
                var contextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
                    .UseSqlite(connection)
                    .Options;

                using(var context = new CarpentryDataContext(contextOptions))
                {
                    context.Database.EnsureCreated();
                    
                    //add default records

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
                Assert.Fail();
                //var dbContext = new CarpentryDataContext(contextOptions);

                //var mockLogger = new Mock<ILogger<DataQueryService>>(MockBehavior.Loose);

                //var queryService = new DataQueryService(dbContext, mockLogger.Object);


            }
            finally
            {
                connection.Close();
            }
            

            //List<DeckCardData> deckCards = new List<DeckCardData>()
            //{
            //    new DeckCardData
            //    {
                    
            //    },
                
            //};


            
            


            //Act


            //Assert
            Assert.Fail();
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


            //Act


            //Assert
            Assert.Fail();
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
