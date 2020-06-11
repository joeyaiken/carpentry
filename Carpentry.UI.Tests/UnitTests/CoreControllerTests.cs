using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
//using Carpentry.Service.Interfaces;
//using Carpentry.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.UnitTests
{
    [TestClass]
    public class CoreControllerTests
    {

        
        [TestMethod]
        public void Core_GetStatus_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
            ////assemble
            //var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);

            //var coreController = new Controllers.CoreController(mockFilterService.Object);

            ////act
            //var response = coreController.GetStatus();

            ////assert
            //Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            //var typedResult = response as OkObjectResult;
            //string resultValue = typedResult.Value as string;

            //Assert.AreEqual("Online", resultValue);
        }

        [TestMethod]
        public async Task Core_GetFilterValues_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
            ////assemble
            //var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);

            //var expectedResult = new AppFiltersDto()
            //{
            //    Formats = new List<FilterOption>(),
            //    ManaColors = new List<FilterOption>(),
            //    Rarities = new List<FilterOption>(),
            //    Sets = new List<FilterOption>(),
            //    Statuses = new List<FilterOption>(),
            //    Types = new List<FilterOption>(),
            //};

            //mockFilterService
            //    .Setup(p => p.GetAppFilterValues())
            //    .ReturnsAsync(expectedResult);

            //var coreController = new Controllers.CoreController(mockFilterService.Object);

            ////act
            //var response = await coreController.GetFilterValues();

            ////assert
            //Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            //var typedResult = response.Result as OkObjectResult;
            //AppFiltersDto resultValue = typedResult.Value as AppFiltersDto;

            //Assert.IsNotNull(resultValue);

            //Assert.IsNotNull(resultValue.Formats);
            //Assert.IsNotNull(resultValue.ManaColors);
            //Assert.IsNotNull(resultValue.Rarities);
            //Assert.IsNotNull(resultValue.Sets);
            //Assert.IsNotNull(resultValue.Statuses);
            //Assert.IsNotNull(resultValue.Types);
        }

        [TestMethod]
        public async Task Core_GetTrackedSets_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_UpdateTrackedSetScryData_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_UpdateTrackedSetCardData_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_GetAllAvailableSets_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_AddTrackedSet_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_RemoveTrackedSet_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_VerifyBackupLocation_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_BackupCollection_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_RestoreCollectionFromBackup_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_ValidateImport_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task Core_AddValidatedImport_ReturnsOK_Test()
        {
            Assert.Fail("Not implemented");
        }

    }
}
