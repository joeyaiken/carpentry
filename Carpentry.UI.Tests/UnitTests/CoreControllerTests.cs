﻿using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.UI.Models;
using Carpentry.UI.Util;
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
        private readonly Controllers.CoreController _coreController;

        public CoreControllerTests()
        {
            //var mockBackupService = new Mock<IDataBackupService>(MockBehavior.Strict);

            var mockFilterService = new Mock<IFilterService>(MockBehavior.Strict);

            mockFilterService
                .Setup(p => p.GetCardStatusFilterOptions())
                .ReturnsAsync(new List<FilterOption>());

            mockFilterService
                .Setup(p => p.GetFormatFilterOptions())
                .ReturnsAsync(new List<FilterOption>());

            mockFilterService
                .Setup(p => p.GetManaColorFilterOptions())
                .ReturnsAsync(new List<FilterOption>());

            mockFilterService
                .Setup(p => p.GetRarityFilterOptions())
                .ReturnsAsync(new List<FilterOption>());

            mockFilterService
                .Setup(p => p.GetSetFilterOptions())
                .ReturnsAsync(new List<FilterOption>());

            mockFilterService
                .Setup(p => p.GetTypeFilterOptions())
                .ReturnsAsync(new List<FilterOption>());

            //var mockMapper = new Mock<IMapperService>(MockBehavior.Strict);

            var mockRefService = Carpentry.Data.Tests.MockClasses.MockDataServices.MockDataReferenceService();

            var mapperService = new MapperService(mockRefService.Object);

            _coreController = new Controllers.CoreController(mockFilterService.Object, mapperService);
        }

        #region Tests - Controller methods all return Ok/Accepted

        [TestMethod]
        public void Core_GetStatus_ReturnsAsyncOK_Test()
        {
            //assemble


            //act
            var response = _coreController.Get();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual( "Online", resultValue);
        }

        [TestMethod]
        public async Task Core_GetFilterValues_ReturnsAsyncOK_Test()
        {
            //assemble


            //act
            var response = await _coreController.GetFilterValues();

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            AppFiltersDto resultValue = typedResult.Value as AppFiltersDto;

            Assert.IsNotNull(resultValue);
            
            Assert.IsNotNull(resultValue.Formats);
            Assert.IsNotNull(resultValue.ManaColors);
            Assert.IsNotNull(resultValue.Rarities);
            Assert.IsNotNull(resultValue.Sets);
            Assert.IsNotNull(resultValue.Statuses);
            Assert.IsNotNull(resultValue.Types);
        }
        
        //[TestMethod]
        //public async Task Core_BackupDatabase_ReturnsAsyncOK_Test()
        //{
        //    //assemble


        //    //act
        //    var response = await _coreController.BackupDatabase();

        //    //assert
        //    Assert.IsInstanceOfType(response, typeof(OkResult));

        //    ////BackupDatabase
        //    //Assert.Fail();
        //}
        
        //[TestMethod]
        //public async Task Core_RestoreDatabase_ReturnsAsyncOK_Test()
        //{
        //    //assemble


        //    //act
        //    var response = await _coreController.RestoreDatabase();

        //    //assert
        //    Assert.IsInstanceOfType(response, typeof(OkResult));
            
        //    ////RestoreDatabase
        //    //Assert.Fail();
        //}
        
        //[TestMethod]
        //public async Task Core_GetDatabaseUpdateStatus_ReturnsAsyncOK_Test()
        //{
        //    //assemble


        //    //act
        //    var response = await _coreController.GetDatabaseUpdateStatus();

        //    //assert
        //    Assert.IsInstanceOfType(response, typeof(OkResult));

        //    ////GetDatabaseUpdateStatus
        //    //Assert.Fail();
        //}
        
        //[TestMethod]
        //public async Task Core_UpdateScryfallSet_ReturnsAsyncOK_Test()
        //{
        //    //assemble
        //    string setToSubmit = "THB";

        //    //act
        //    var response = await _coreController.UpdateScryfallSet(setToSubmit);

        //    //assert
        //    Assert.IsInstanceOfType(response, typeof(OkResult));

        //    ////UpdateScryfallSet
        //    //Assert.Fail();
        //}
        
        //[TestMethod]
        //public async Task Core_UpdateSetData_ReturnsAsyncOK_Test()
        //{
        //    //assemble
        //    string setToSubmit = "THB";

        //    //act
        //    var response = await _coreController.UpdateSetData(setToSubmit);

        //    //assert
        //    Assert.IsInstanceOfType(response, typeof(OkResult));

        //    ////UpdateSetData
        //    //Assert.Fail();
        //}

        #endregion

    }
}
