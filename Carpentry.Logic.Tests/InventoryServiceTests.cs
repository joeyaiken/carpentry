using Carpentry.CarpentryData;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class InventoryServiceTests : CarpentryServiceTestBase
    {
        protected override bool SeedViews => false;
        private InventoryService _inventoryService = null!;
        private Mock<ILogger<InventoryService>> _mockLogger = null!;

        protected override Task BeforeEachChild() => Task.CompletedTask;

        protected override Task AfterEachChild() => Task.CompletedTask;

        protected override void ResetContextChild()
        {
            _mockLogger = new Mock<ILogger<InventoryService>>();
            _inventoryService = new InventoryService(CardContext, _mockLogger.Object);//context, logger
        }

        [TestMethod]
        public void InventoryServiceTests_AreImplemented_Test()
        {
            Assert.Fail("Not implemented");
        }

    }
}
