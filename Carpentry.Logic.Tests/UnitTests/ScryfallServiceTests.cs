﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Carpentry.Logic.Tests.UnitTests
{
    [TestClass]
    public class ScryfallServiceTests
    {

        private readonly ScryfallService _scryService;

        public ScryfallServiceTests()
        {
            var handlerMock = new Mock<HttpMessageHandler>(); //MockBehavior.Strict

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

            _scryService = new ScryfallService(httpClient);
        }
        
        [TestMethod]
        public async Task ScryfallService_GetFullSet_Test()
        {
            //Assemble
            string codeToSearch = "THB";

            //Act
            var result = await _scryService.GetFullSet(codeToSearch);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.CardTokens.Count > 100);


        }

    }
}
