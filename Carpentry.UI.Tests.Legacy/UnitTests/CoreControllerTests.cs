using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using Carpentry.UI.Legacy.Controllers;
using Carpentry.UI.Legacy.Models;
using Carpentry.UI.Legacy.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.Legacy.UnitTests
{
    [TestClass]
    public class CoreControllerTests
    {
        [TestMethod]
        public void Core_GetStatus_ReturnsAsyncOK_Test()
        {
            //assemble
            var mockCoreService = new Mock<ICoreControllerService>(MockBehavior.Strict);

            var mapperService = new MapperService();

            var coreController = new CoreController(mockCoreService.Object, mapperService);

            //act
            var response = coreController.Get();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual("Online", resultValue);
        }

        [TestMethod]
        public async Task Core_GetFilterValues_ReturnsAsyncOK_Test()
        {
            //assemble
            var mockCoreService = new Mock<ICoreControllerService>(MockBehavior.Strict);

            var expectedResult = new AppFiltersDto()
            {
                Formats = new List<FilterOptionDto>(),
                ManaColors = new List<FilterOptionDto>(),
                Rarities = new List<FilterOptionDto>(),
                Sets = new List<FilterOptionDto>(),
                Statuses = new List<FilterOptionDto>(),
                Types = new List<FilterOptionDto>(),
            };

            mockCoreService
                .Setup(p => p.GetAppFilterValues())
                .ReturnsAsync(expectedResult);

            var mapperService = new MapperService();

            var coreController = new CoreController(mockCoreService.Object, mapperService);

            //act
            var response = await coreController.GetFilterValues();

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            LegacyAppFiltersDto resultValue = typedResult.Value as LegacyAppFiltersDto;

            Assert.IsNotNull(resultValue);

            Assert.IsNotNull(resultValue.Formats);
            Assert.IsNotNull(resultValue.ManaColors);
            Assert.IsNotNull(resultValue.Rarities);
            Assert.IsNotNull(resultValue.Sets);
            Assert.IsNotNull(resultValue.Statuses);
            Assert.IsNotNull(resultValue.Types);
        }
    }
}
