using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Tests.IntegrationTests
{
    [TestClass]
    public class InventoryDetailTests
    {
        readonly CarpentryFactory _factory;

        public InventoryDetailTests()
        {
            _factory = new CarpentryFactory();
        }

        [TestMethod]
        public async Task InventoryDetail_JornSnow_Works()
        {
            //Arrange
            var cardId = 1110;
            string apiEndpointInventoryDetail = $"api/Inventory/GetInventoryDetail?cardId={cardId}";
            var client = _factory.CreateClient();

            //act
            var response = await client.GetAsync(apiEndpointInventoryDetail);
            var responseContent = await response.Content.ReadAsStringAsync();
            InventoryDetailDto searchResult = JsonConvert.DeserializeObject<InventoryDetailDto>(responseContent);

            //assert
            Assert.IsNotNull(searchResult);
        }
    }
}
