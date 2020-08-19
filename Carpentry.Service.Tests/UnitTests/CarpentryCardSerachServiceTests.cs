using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Scryfall;
using Carpentry.Logic.Search;
using Carpentry.Service.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class CarpentryCardSerachServiceTests
    {
        [TestMethod]
        public async Task CarpentryCardSerachService_SearchInventory_Test()
        {
            //Assemble
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);
            var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            var expectedResult = new List<CardSearchResultDto>()
            {

            };

            mockSearchService
                .Setup(p => p.SearchCardDefinitions(It.IsNotNull<CardSearchQueryParameter>()))
                .ReturnsAsync(expectedResult);

            var cardSearchService = new CarpentryCardSearchService(
                mockSearchService.Object,
                mockScryService.Object
                );

            var queryParamToSubmit = new CardSearchQueryParameter();

            //Act
            var result = await cardSearchService.SearchInventory(queryParamToSubmit);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Count, result.Count);
        }
        
        //[TestMethod]
        //public async Task CarpentryCardSerachService_SearchWeb_Test()
        //{
        //    var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);
        //    var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

        //    var expectedResult = new List<ScryfallMagicCard>()
        //    {

        //    };

        //    mockScryService
        //        .Setup(p => p.SearchScryfallByName(It.IsNotNull<string>(), It.IsAny<bool>()))
        //        .ReturnsAsync(expectedResult);

        //    var cardSearchService = new CarpentryCardSearchService(
        //        mockSearchService.Object,
        //        mockScryService.Object
        //        );

        //    var queryParam = new NameSearchQueryParameter()
        //    {
        //        Name = "Opt",
        //        Exclusive = false,
        //    };


        //    //Act
        //    var result = await cardSearchService.SearchWeb(queryParam);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(expectedResult.Count, result.Count);
        //}
    }
}
