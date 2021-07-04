using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Models;
using System.Net.Http;
using Moq;
using Moq.Protected;
using System.Threading;
using System.Net;
using Microsoft.Extensions.Logging;
using Carpentry.ScryfallData;
using System.Linq;

namespace Carpentry.Logic.Tests.UnitTests
{
    [TestClass]
    public class ScryfallServiceTests
    {
        private ScryfallService _scryService;

        public ScryfallServiceTests()
        {
            //var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict); //MockBehavior.Strict

            ////Got this approach from the following article:
            ////https://gingter.org/2018/07/26/how-to-mock-httpclient-in-your-net-c-unit-tests/
            //handlerMock
            //    .Protected()
            //   .Setup<Task<HttpResponseMessage>>(
            //        "SendAsync",
            //        ItExpr.IsAny<HttpRequestMessage>(),
            //        ItExpr.IsAny<CancellationToken>()
            //   )
            //   .ReturnsAsync((HttpRequestMessage r, CancellationToken c) => new HttpResponseMessage()
            //   {
            //       StatusCode = HttpStatusCode.OK,
            //       Content = new StringContent(MockHttpClient.HandleMockClientRequest(r.RequestUri.ToString())),
            //   })
            //   .Verifiable();

            //var httpClient = new HttpClient(handlerMock.Object)
            //{
            //    BaseAddress = new Uri("http://test.com/"),
            //};

            //_scryService = new ScryfallService(httpClient);
        }

        [TestInitialize]
        public void BeforeEach()
        {
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

            //var mockRepo = new Mock<ScryfallDataContext>();

            _scryService = new ScryfallService(
                //mockRepo.Object, 
                mockLogger.Object, httpClient);

        }

        [DataRow(true, 262)]
        [DataRow(false, 683)]
        [DataTestMethod]
        public async Task GetAvailableSets_Works_Test(bool isFiltered, int expectedCount)
        {
            var result = await _scryService.GetAvailableSets(isFiltered);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            //TODO - Assert DB record exists & is updated (start by seeding something with no data?)
        }

        //Test: Task<ScryfallSetDataDto> GetSetCards(string setCode);
        [DataRow("MH2", 492)]
        [DataRow("THB", 358)]
        [DataRow("WAR", 311)]
        [DataTestMethod]
        public async Task GetSetCards_Works_Test(string setCode, int expectedCount)
        {
            var result = await _scryService.GetSetCards(setCode);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.CardTokens.Count > 100);
            Assert.AreEqual(expectedCount, result.CardTokens.Count);
        }

        //Test: Task EnsureSetsUpdated(List<string> setCodes);
        //[TestMethod]
        [DataRow(new string[] { "MH2", "THB", "WAR" })]
        [DataTestMethod]
        public async Task EnsureSetsUpdated_Works_Test(string[] codesToUpdate)
        {
            //var codesToUpdate = new List<string>() {"MH2","THB","WAR"};
            await _scryService.EnsureSetsUpdated(codesToUpdate.ToList());
            Assert.Fail("Must make assertions about the actual database");
        }
    }
}
