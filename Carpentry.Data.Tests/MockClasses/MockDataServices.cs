using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
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

            List<MagicFormatData> mockFormats = new List<MagicFormatData>
            {
                new MagicFormatData { Id = 0, Name = "modern" },
                new MagicFormatData { Id = 1, Name = "standard" },
                new MagicFormatData { Id = 2, Name = "commander" },
                new MagicFormatData { Id = 3, Name = "brawl" },
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
