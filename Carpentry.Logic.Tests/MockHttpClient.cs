using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Tests
{
    public class MockHttpClient
    {
        public static string HandleMockClientRequest(string requestString)
        {
            switch (requestString)
            {
                case "https://api.scryfall.com/sets/THB":
                    return LoadMockResponse("THB1");
                case "https://api.scryfall.com/cards/search?order=set&q=e%3Athb&unique=prints":
                    return LoadMockResponse("THB2");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=2&q=e%3Athb&unique=prints":
                    return LoadMockResponse("THB3");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=3&q=e%3Athb&unique=prints":
                    return LoadMockResponse("THB4");
                case "https://api.scryfall.com/sets":
                    return LoadMockResponse("SETS");
                case "https://api.scryfall.com/sets/WAR":
                    return LoadMockResponse("WAR1");
                case "https://api.scryfall.com/cards/search?order=set&q=e%3Awar&unique=prints":
                    return LoadMockResponse("WAR2");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=2&q=e%3Awar&unique=prints":
                    return LoadMockResponse("WAR3");
                case "https://api.scryfall.com/sets/MH2":
                    return LoadMockResponse("mh2_1");
                case "https://api.scryfall.com/cards/search?order=set&q=e%3Amh2&unique=prints":
                    return LoadMockResponse("mh2_2");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=2&q=e%3Amh2&unique=prints":
                    return LoadMockResponse("mh2_3");
                case "https://api.scryfall.com/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page=3&q=e%3Amh2&unique=prints":
                    return LoadMockResponse("mh2_4");
                default:
                    throw new Exception("Unexpected request string");
            }
        }

        public static string LoadMockResponse(string fileName)
        {
            string filepath = $"C:\\DotNet\\Carpentry\\Carpentry.Logic.Tests\\MockServiceResponses\\{fileName}.txt";
            string dataString = System.IO.File.ReadAllText(filepath);
            return dataString;
        }
    }
}
