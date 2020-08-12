using Carpentry.Logic.Models;
using Carpentry.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.UnitTests
{
    [TestClass]
    public class CoreControllerTests
    {
        [TestMethod]
        public void Core_GetStatus_ReturnsOK_Test()
        {
            //assemble
            var mockCoreService = new Mock<ICarpentryCoreService>(MockBehavior.Strict);

            var coreController = new Controllers.CoreController(mockCoreService.Object);

            //act
            var response = coreController.GetStatus();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual("Online", resultValue);
        }

        [TestMethod]
        public async Task Core_GetFilterValues_ReturnsOK_Test()
        {
            //assemble
            var mockCoreService = new Mock<ICarpentryCoreService>(MockBehavior.Strict);

            var expectedResult = new AppFiltersDto()
            {
                Formats = new List<FilterOption>(),
                Colors = new List<FilterOption>(),
                Rarities = new List<FilterOption>(),
                Sets = new List<FilterOption>(),
                Statuses = new List<FilterOption>(),
                Types = new List<FilterOption>(),
            };

            mockCoreService
                .Setup(p => p.GetAppFilterValues())
                .ReturnsAsync(expectedResult);

            var coreController = new Controllers.CoreController(mockCoreService.Object);

            //act
            var response = await coreController.GetFilterValues();

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            AppFiltersDto resultValue = typedResult.Value as AppFiltersDto;

            Assert.IsNotNull(resultValue);

            Assert.IsNotNull(resultValue.Formats);
            Assert.IsNotNull(resultValue.Colors);
            Assert.IsNotNull(resultValue.Rarities);
            Assert.IsNotNull(resultValue.Sets);
            Assert.IsNotNull(resultValue.Statuses);
            Assert.IsNotNull(resultValue.Types);
        }

        [TestMethod]
        public async Task Core_GetTrackedSets_ReturnsOK_Test()
        {
            //assemble
            var mockCoreService = new Mock<ICarpentryCoreService>(MockBehavior.Strict);

            List<SetDetailDto> expectedResult = new List<SetDetailDto>()
            {
                new SetDetailDto(),
                new SetDetailDto(),
                new SetDetailDto(),
            };

            mockCoreService.
                Setup(p => p.GetTrackedSets(It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(expectedResult);

            var coreController = new Controllers.CoreController(mockCoreService.Object);

            //act
            var response = await coreController.GetTrackedSets(false, false);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            List<SetDetailDto> resultValue = typedResult.Value as List<SetDetailDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.Count, expectedResult.Count);
        }

        [TestMethod]
        public async Task Core_AddTrackedSet_ReturnsOK_Test()
        {
            //assemble
            var mockCoreService = new Mock<ICarpentryCoreService>(MockBehavior.Strict);

            int setIdToTrack = 1;

            mockCoreService.
                Setup(p => p.AddTrackedSet(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var coreController = new Controllers.CoreController(mockCoreService.Object);

            //act
            var response = await coreController.AddTrackedSet(setIdToTrack);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Core_UpdateTrackedSet_ReturnsOK_Test()
        {
            //assemble
            var mockCoreService = new Mock<ICarpentryCoreService>(MockBehavior.Strict);

            int setIdToTrack = 1;

            mockCoreService.
                Setup(p => p.UpdateTrackedSet(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var coreController = new Controllers.CoreController(mockCoreService.Object);

            //act
            var response = await coreController.UpdateTrackedSet(setIdToTrack);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Core_RemoveTrackedSet_ReturnsOK_Test()
        {
            //assemble
            var mockCoreService = new Mock<ICarpentryCoreService>(MockBehavior.Strict);

            int setIdToTrack = 1;

            mockCoreService.
                Setup(p => p.RemoveTrackedSet(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var coreController = new Controllers.CoreController(mockCoreService.Object);

            //act
            var response = await coreController.RemoveTrackedSet(setIdToTrack);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

    }
}
