using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.Tests.MockClasses
{
    public static class MockDataServices
    {

        public static Mock<IDataReferenceService> MockDataReferenceService()
        {
            MagicFormatData x = new MagicFormatData
            {
                Id = 0,
                Name = "Modern",
            };

            List<DataReferenceValue<int>> mockFormats = new List<DataReferenceValue<int>>
            {
                new DataReferenceValue<int> { Id = 0, Name = "modern" },
                new DataReferenceValue<int> { Id = 1, Name = "standard" },
                new DataReferenceValue<int> { Id = 2, Name = "commander" },
                new DataReferenceValue<int> { Id = 3, Name = "brawl" },
            };


            var mockService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.GetMagicFormat(It.IsAny<string>()))
                .ReturnsAsync(mockFormats[0]);
            mockService
                .Setup(p => p.GetMagicFormat(It.IsAny<int>()))
                .ReturnsAsync(mockFormats[0]);
            mockService
                .Setup(p => p.GetAllMagicFormats())
                .ReturnsAsync(mockFormats);

            return mockService;
        }

    }
}
