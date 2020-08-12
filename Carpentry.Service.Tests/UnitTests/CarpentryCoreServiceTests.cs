using Carpentry.Logic.Interfaces;
using Carpentry.Service.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Logic.Models;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class CarpentryCoreServiceTests
    {
        #region Filter Options

        [TestMethod]
        public async Task CarpentryCoreService_GetAppFilterValues_Test()
        {
            //Assemble
            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);

            var mockFilterResponse = new AppFiltersDto()
            {
                Colors = new List<FilterOption>(),
                Formats = new List<FilterOption>(),
                Rarities = new List<FilterOption>(),
                Sets = new List<FilterOption>(),
                Statuses = new List<FilterOption>(),
                Types = new List<FilterOption>(),
            };

            mockFilterService
                .Setup(p => p.GetAppFilterValues())
                .ReturnsAsync(mockFilterResponse);

            var coreService = new CarpentryCoreService(
                mockDataUpdateService.Object,
                mockFilterService.Object);

            //Act
            var result = await coreService.GetAppFilterValues();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Colors);
            Assert.IsNotNull(result.Formats);
            Assert.IsNotNull(result.Rarities);
            Assert.IsNotNull(result.Sets);
            Assert.IsNotNull(result.Statuses);
            Assert.IsNotNull(result.Types);
        }

        #endregion

        #region Tracked Sets

        [TestMethod]
        public async Task CarpentryCoreService_GetTrackedSets_Test()
        {
            //Assemble
            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);

            var showUntrackedVal = true;
            var updateVal = true;

            List<SetDetailDto> mockResult = new List<SetDetailDto>()
            {
                new SetDetailDto(),
                new SetDetailDto(),
                new SetDetailDto(),
            };

            mockDataUpdateService
                .Setup(p => p.GetTrackedSets(It.Is<bool>(b => b == showUntrackedVal), It.Is<bool>(b => b == updateVal)))
                .ReturnsAsync(mockResult);

            var coreService = new CarpentryCoreService(
                mockDataUpdateService.Object,
                mockFilterService.Object);

            //Act
            var result = await coreService.GetTrackedSets(showUntrackedVal, updateVal);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockResult.Count, result.Count);

        }

        [TestMethod]
        public async Task CarpentryCoreService_AddTrackedSet_Test()
        {
            //Assemble
            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);

            List<SetDetailDto> mockResult = new List<SetDetailDto>()
            {
                new SetDetailDto(),
                new SetDetailDto(),
                new SetDetailDto(),
            };

            mockDataUpdateService
                .Setup(p => p.AddTrackedSet(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var coreService = new CarpentryCoreService(
                mockDataUpdateService.Object,
                mockFilterService.Object);

            int setIdToSubmit = 1;

            //Act
            await coreService.AddTrackedSet(setIdToSubmit);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task CarpentryCoreService_UpdateTrackedSet_Test()
        {
            //Assemble
            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);

            List<SetDetailDto> mockResult = new List<SetDetailDto>()
            {
                new SetDetailDto(),
                new SetDetailDto(),
                new SetDetailDto(),
            };

            mockDataUpdateService
                .Setup(p => p.UpdateTrackedSet(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var coreService = new CarpentryCoreService(
                mockDataUpdateService.Object,
                mockFilterService.Object);

            int setIdToSubmit = 1;

            //Act
            await coreService.UpdateTrackedSet(setIdToSubmit);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task CarpentryCoreService_RemoveTrackedSet_Test()
        {
            //Assemble
            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);

            List<SetDetailDto> mockResult = new List<SetDetailDto>()
            {
                new SetDetailDto(),
                new SetDetailDto(),
                new SetDetailDto(),
            };

            mockDataUpdateService
                .Setup(p => p.RemoveTrackedSet(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var coreService = new CarpentryCoreService(
                mockDataUpdateService.Object,
                mockFilterService.Object);

            int setIdToSubmit = 1;

            //Act
            await coreService.RemoveTrackedSet(setIdToSubmit);

            //Assert
            //Nothing to assert, returns void
        }

        #endregion
    }
}
