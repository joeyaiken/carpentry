using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests.UnitTests
{
    [TestClass]
    public class FilterServiceTests
    {
        [TestMethod]
        public async Task FilterService_GetAppFilterValues_Test()
        {
            //Arrange
            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            mockReferenceService
                .Setup(p => p.GetAllMagicFormats())
                .ReturnsAsync(new List<DataReferenceValue<int>>());

            mockReferenceService
                .Setup(p => p.GetAllManaColors())
                .ReturnsAsync(new List<DataReferenceValue<char>>());

            mockReferenceService
                .Setup(p => p.GetAllRarities())
                .ReturnsAsync(new List<DataReferenceValue<char>>());

            mockReferenceService
                .Setup(p => p.GetAllSets())
                .ReturnsAsync(new List<DataReferenceValue<int>>());

            mockReferenceService
                .Setup(p => p.GetAllStatuses())
                .ReturnsAsync(new List<DataReferenceValue<int>>());

            mockReferenceService
                .Setup(p => p.GetAllTypes())
                .Returns(new List<DataReferenceValue<string>>());

            FilterService service = new FilterService(mockReferenceService.Object);

            //Act
            AppFiltersDto resultValue = await service.GetAppFilterValues();

            //Assert
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
