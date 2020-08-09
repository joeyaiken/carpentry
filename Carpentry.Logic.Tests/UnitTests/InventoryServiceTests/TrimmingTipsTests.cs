using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests.UnitTests.InventoryServiceTests
{
    [TestClass]
    public class TrimmingTipsTests
    {
        //Trimming Tips haven't been implemented.  For now, I expect this method to throw Not Implemented Exceptions.
        [TestMethod]
        public async Task InventoryService_GetTrimmingTips_ThrowsNotImplemented_Test()
        {
            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);
            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);
            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);
            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //Act
            
            //Assert
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => inventoryService.GetTrimmingTips());

        }

        //Trimming Tips haven't been implemented.  For now, I expect this method to throw Not Implemented Exceptions.
        [TestMethod]
        public async Task InventoryService_HideTrimmingTip_ThrowsNotImplemented_Test()
        {
            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);
            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);
            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);
            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockCoreRepo.Object,
                mockCardDatarepo.Object);

            var payloadToSubmit = new InventoryOverviewDto() { };

            //Act

            //Assert
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => inventoryService.HideTrimmingTip(payloadToSubmit));
        }
    }
}
