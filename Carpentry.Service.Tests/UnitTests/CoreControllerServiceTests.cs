using Carpentry.Data.Interfaces;
using Carpentry.Service.Implementations;
using Carpentry.Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class CoreControllerServiceTests
    {
        //Task<AppFiltersDto> GetAppFilterValues();
        [TestMethod]
        public async Task CoreControllerService_GetAppFilterValues_Test() 
        {
            //Arrange
            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            //AppFiltersDto expectedResult = new AppFiltersDto();

            //mockReferenceService
            //    .Setup(p => p.GetAllCardVariantTypes())
            //    .ReturnsAsync(new List<Data.DataModels.CardVariantTypeData>())



            CoreControllerService service = new CoreControllerService(mockReferenceService.Object);

            //Act
            var resultValue = await service.GetAppFilterValues();

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
