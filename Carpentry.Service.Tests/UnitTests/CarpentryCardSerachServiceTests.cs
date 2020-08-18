using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Service.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class CarpentryCardSerachServiceTests
    {
        [TestMethod]
        public void CarpentryCardSerachService_SearchInventory_Test()
        {
            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);
            var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            var cardSearchService = new CarpentryCardSearchService(
                mockSearchService.Object,
                mockScryService.Object
                );

            var queryParam = new NameSearchQueryParameter();

            var result = cardSearchService.SearchWeb(queryParam);

            Assert.Fail();
        }
        
        [TestMethod]
        public void CarpentryCardSerachService_SearchWeb_Test()
        {
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);
            var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            var cardSearchService = new CarpentryCardSearchService(
                mockSearchService.Object,
                mockScryService.Object
                );

            var queryParam = new NameSearchQueryParameter();

            var result = cardSearchService.SearchWeb(queryParam);

            Assert.Fail();

        }
    }
}
