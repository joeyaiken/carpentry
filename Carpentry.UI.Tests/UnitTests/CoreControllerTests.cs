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
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

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
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var expectedResult = new AppFiltersDto()
            {
                Formats = new List<FilterOption>(),
                Colors = new List<FilterOption>(),
                Rarities = new List<FilterOption>(),
                Sets = new List<FilterOption>(),
                Statuses = new List<FilterOption>(),
                Types = new List<FilterOption>(),
            };

            mockFilterService
                .Setup(p => p.GetAppFilterValues())
                .ReturnsAsync(expectedResult);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

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
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            List<SetDetailDto> expectedResult = new List<SetDetailDto>()
            {
                new SetDetailDto(),
                new SetDetailDto(),
                new SetDetailDto(),
            };

            mockUpdateService.
                Setup(p => p.GetTrackedSets())
                .ReturnsAsync(expectedResult);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            //act
            var response = await coreController.GetTrackedSets();

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            List<SetDetailDto> resultValue = typedResult.Value as List<SetDetailDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.Count, expectedResult.Count);
        }

        [TestMethod]
        public async Task Core_UpdateTrackedSetScryData_ReturnsOK_Test()
        {
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            mockUpdateService
                .Setup(p => p.UpdateTrackedSetScryData(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            var setCodeToRequest = "IKO";

            //act
            var response = await coreController.UpdateTrackedSetScryData(setCodeToRequest);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Core_UpdateTrackedSetCardData_ReturnsOK_Test()
        {
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            mockUpdateService
                .Setup(p => p.UpdateTrackedSetCardData(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            var setCodeToRequest = "IKO";

            //act
            var response = await coreController.UpdateTrackedSetCardData(setCodeToRequest);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Core_GetAllAvailableSets_ReturnsOK_Test()
        {
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            List<SetDetailDto> expectedResult = new List<SetDetailDto>()
            {
                new SetDetailDto(),
                new SetDetailDto(),
                new SetDetailDto(),
            };

            mockUpdateService.
                Setup(p => p.GetAllAvailableSets())
                .ReturnsAsync(expectedResult);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            //act
            var response = await coreController.GetAllAvailableSets();

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
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            mockUpdateService
                .Setup(p => p.AddTrackedSet(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            var setCodeToRequest = "IKO";

            //act
            var response = await coreController.AddTrackedSet(setCodeToRequest);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Core_RemoveTrackedSet_ReturnsOK_Test()
        {
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            mockUpdateService
                .Setup(p => p.RemoveTrackedSet(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            var setCodeToRequest = "IKO";

            //act
            var response = await coreController.RemoveTrackedSet(setCodeToRequest);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Core_VerifyBackupLocation_ReturnsOK_Test()
        {
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var expectedResult = new BackupDetailDto();

            mockBackupService
                .Setup(p => p.VerifyBackupLocation(It.IsAny<string>()))
                .ReturnsAsync(expectedResult);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            string directoryToSubmit = "c:\\some\\directory\\";

            //act
            var response = await coreController.VerifyBackupLocation(directoryToSubmit);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            BackupDetailDto resultValue = typedResult.Value as BackupDetailDto;

            Assert.IsNotNull(resultValue);
        }

        [TestMethod]
        public async Task Core_BackupCollection_ReturnsOK_Test()
        {
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            mockBackupService
                .Setup(p => p.BackupCollection(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            string backupDirectory = "C:\\Something\\";

            //act
            var response = await coreController.BackupCollection(backupDirectory);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Core_RestoreCollectionFromBackup_ReturnsOK_Test()
        {
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            mockBackupService
                .Setup(p => p.RestoreCollectionFromBackup(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            string backupDirectory = "C:\\Something\\";

            //act
            var response = await coreController.RestoreCollectionFromBackup(backupDirectory);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Core_ValidateImport_ReturnsOK_Test()
        {
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            CardImportDto payloadToSubmit = new CardImportDto();

            ValidatedCardImportDto expectedResponse = new ValidatedCardImportDto();

            mockImportService
                .Setup(p => p.ValidateImport(It.IsAny<CardImportDto>()))
                .ReturnsAsync(expectedResponse);
            
            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            //act
            var response = await coreController.ValidateImport(payloadToSubmit);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            ValidatedCardImportDto resultValue = typedResult.Value as ValidatedCardImportDto;

            Assert.IsNotNull(resultValue);
        }

        [TestMethod]
        public async Task Core_AddValidatedImport_ReturnsOK_Test()
        {
            //assemble
            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);
            var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);
            var mockImportService = new Mock<ICardImportService>(MockBehavior.Strict);
            var mockUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var payloadToSubmit = new ValidatedCardImportDto();

            mockImportService
                .Setup(p => p.AddValidatedImport(It.IsAny<ValidatedCardImportDto>()))
                .Returns(Task.CompletedTask);


            var coreController = new Controllers.CoreController(mockFilterService.Object, mockBackupService.Object, mockImportService.Object, mockUpdateService.Object);

            //act
            var response = await coreController.AddValidatedImport(payloadToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

    }
}
