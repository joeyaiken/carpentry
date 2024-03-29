﻿using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    public class MockHttpClient : Mock<HttpMessageHandler>
    {
        public readonly HttpClient HttpClient;

        public MockHttpClient() : base(MockBehavior.Strict)
        {
            //Got this approach from the following article:
            //https://gingter.org/2018/07/26/how-to-mock-httpclient-in-your-net-c-unit-tests/
            
            this.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage r, CancellationToken c) => new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(HandleMockClientRequest(r.RequestUri.ToString())),
                })
                .Verifiable();

            HttpClient = new HttpClient(Object)
            {

                BaseAddress = new Uri("http://test.com/"),
            };
        }
        private static string HandleMockClientRequest(string requestString)
        {
            switch (requestString)
            {
                case "https://api.scryfall.com/sets/thb":
                    return LoadMockResponse("THB1");
                case "https://api.scryfall.com/cards/search?order=set&q=e%3Athb&unique=prints":
                    return LoadMockResponse("THB2");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=2&q=e%3Athb&unique=prints":
                    return LoadMockResponse("THB3");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=3&q=e%3Athb&unique=prints":
                    return LoadMockResponse("THB4");
                case "https://api.scryfall.com/sets":
                    return LoadMockResponse("SETS");
                case "https://api.scryfall.com/sets/war":
                    return LoadMockResponse("WAR1");
                case "https://api.scryfall.com/cards/search?order=set&q=e%3Awar&unique=prints":
                    return LoadMockResponse("WAR2");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=2&q=e%3Awar&unique=prints":
                    return LoadMockResponse("WAR3");
                case "https://api.scryfall.com/sets/mh2":
                    return LoadMockResponse("mh2_1");
                case "https://api.scryfall.com/cards/search?order=set&q=e%3Amh2&unique=prints":
                    return LoadMockResponse("mh2_2");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=2&q=e%3Amh2&unique=prints":
                    return LoadMockResponse("mh2_3");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=3&q=e%3Amh2&unique=prints":
                    return LoadMockResponse("mh2_4");

                case "https://api.scryfall.com/sets/eld":
                    return LoadMockResponse("eld_1");
                case "https://api.scryfall.com/cards/search?order=set&q=e%3Aeld&unique=prints":
                    return LoadMockResponse("eld_2");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=2&q=e%3Aeld&unique=prints":
                    return LoadMockResponse("eld_3");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=3&q=e%3Aeld&unique=prints":
                    return LoadMockResponse("eld_4");

                default:
                    throw new Exception("Unexpected request string");
            }
        }

        private static string LoadMockResponse(string fileName)
        {
            string filepath = $"C:\\DotNet\\Carpentry\\Carpentry.Logic.Tests\\MockServiceResponses\\{fileName}.txt";
            string dataString = System.IO.File.ReadAllText(filepath);
            return dataString;
        }
    }
}
