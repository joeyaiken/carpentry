using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Moq;
using Moq.Protected;
using System.Threading;
using System.Net;
using Microsoft.Extensions.Logging;
using Carpentry.ScryfallData;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Carpentry.ScryfallData.Models;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class ScryfallServiceTests// : CarpentryServiceTestBase
    {
        private ScryfallService _scryService;
        private ScryfallDataContext _scryContext;
        private DbContextOptions<ScryfallDataContext> _contextOptions;

        private void ResetContext()
        {
            _scryContext = new ScryfallDataContext(_contextOptions);

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict); //MockBehavior.Strict

            //Got this approach from the following article:
            //https://gingter.org/2018/07/26/how-to-mock-httpclient-in-your-net-c-unit-tests/
            handlerMock
                .Protected()
               .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync((HttpRequestMessage r, CancellationToken c) => new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(MockHttpClient.HandleMockClientRequest(r.RequestUri.ToString())),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var mockLogger = new Mock<ILogger<ScryfallService>>();

            _scryService = new ScryfallService(_scryContext, mockLogger.Object, httpClient);
        }

        [TestInitialize]
        public async Task BeforeEach()
        {
            _contextOptions = new DbContextOptionsBuilder<ScryfallDataContext>().UseSqlite("Filename=ScryData.db").Options;
            ResetContext();
            await _scryContext.Database.EnsureCreatedAsync();
            ResetContext();
        }

        [TestCleanup]
        public async Task AfterEach()
        {
            await _scryContext.Database.EnsureDeletedAsync();
            await _scryContext.DisposeAsync();
        }

        [DataRow(true, false, 262)]  //Filtered, not seeded
        [DataRow(true, true, 262)]   //Filtered AND seeded
        [DataRow(false, false, 683)] //Not filtered, not seeded
        [DataRow(false, true, 683)]  //Not filtered, IS seeded
        [DataTestMethod]
        public async Task GetSetOverviews_Works_Test(bool isFiltered, bool isSeeded,  int expectedCount) // add 'isSeeded' ?
        {
            if (isSeeded)
            {
                var seedRecord = new ScryfallAuditData()
                {
                    LastUpdated = DateTime.Now.AddDays(-7),
                    SetTokensString = null,
                };
                _scryContext.ScryfallAuditData.Add(seedRecord);
                await _scryContext.SaveChangesAsync();
                ResetContext();
            }

            //If there is an AuditData record in the DB, it should be updated with the new string
            //If there is NO audit data record, one should be created
            //No sets should be created in the process of this test, just the single audit record

            var result = await _scryService.GetSetOverviews(isFiltered);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);

            //Audit record should exist
            //No actual sets should exist
            var auditRecords = await _scryContext.ScryfallAuditData.ToListAsync();
            var scrySetCount = await _scryContext.Sets.CountAsync();

            Assert.AreEqual(1, auditRecords.Count);
            Assert.AreEqual(0, scrySetCount);
            Assert.IsTrue(auditRecords.Single().LastUpdated > DateTime.Today);
            Assert.IsNotNull(auditRecords.Single().SetTokensString);
        }

        //Test: Task<ScryfallSetDataDto> GetSetCards(string setCode);
        [DataRow("MH2", 492)]
        [DataRow("THB", 358)]
        [DataRow("WAR", 311)]
        [DataTestMethod]
        public async Task GetSetDetail_Works_Test(string setCode, int expectedCount)
        {
            var result = await _scryService.GetSetDetail(setCode);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Overview);
            Assert.IsTrue(result.Cards.Count > 100);
            Assert.AreEqual(expectedCount, result.Cards.Count);

            var auditRecordCount = await _scryContext.ScryfallAuditData.CountAsync();
            var scrySetCount = await _scryContext.Sets.CountAsync();

            Assert.AreEqual(0, auditRecordCount);
            Assert.AreEqual(1, scrySetCount);
        }

        [TestMethod]
        public async Task GetSetDetail_HandlesAdventureCardOracleText_Test()
        {
            var result = await _scryService.GetSetDetail("ELD");
            var firstAdventureCard = result.Cards.First(card => card.Name.Contains("//"));
            Assert.IsNotNull(firstAdventureCard.Text);
        }


        ////Test: Task EnsureSetsUpdated(List<string> setCodes);
        ////[TestMethod]
        //[DataRow(new string[] { "MH2", "THB", "WAR" })]
        //[DataTestMethod]
        //public async Task EnsureSetsUpdated_Works_Test(string[] codesToUpdate)
        //{
        //    //var codesToUpdate = new List<string>() {"MH2","THB","WAR"};
        //    await _scryService.EnsureSetsUpdated(codesToUpdate.ToList());
        //    Assert.Fail("Must make assertions about the actual database");
        //}
    }
}
