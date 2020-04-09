using Carpentry.Logic.Interfaces;
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
            var mockBackupService = new Mock<IDataBackupService>();









            _coreController = new Controllers.CoreController();
        }

        #region Tests - Controller methods all return Ok/Accepted

        [TestMethod]
        public async Task Core_GetFilterValues_ReturnsOK_Test()
        {
            //assemble


            //act
            var response = await _coreController.GetFilterValues();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

            ////GetFilterValues
            //Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_BackupDatabase_ReturnsOK_Test()
        {
            //assemble


            //act
            var response = await _coreController.BackupDatabase();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

            ////BackupDatabase
            //Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_RestoreDatabase_ReturnsOK_Test()
        {
            //assemble


            //act
            var response = await _coreController.RestoreDatabase();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
            
            ////RestoreDatabase
            //Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_GetDatabaseUpdateStatus_ReturnsOK_Test()
        {
            //assemble


            //act
            var response = await _coreController.GetDatabaseUpdateStatus();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

            ////GetDatabaseUpdateStatus
            //Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_UpdateScryfallSet_ReturnsOK_Test()
        {
            //assemble
            string setToSubmit = "THB";

            //act
            var response = await _coreController.UpdateScryfallSet(setToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

            ////UpdateScryfallSet
            //Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_UpdateSetData_ReturnsOK_Test()
        {
            //assemble
            string setToSubmit = "THB";

            //act
            var response = await _coreController.UpdateSetData(setToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

            ////UpdateSetData
            //Assert.Fail();
        }

        #endregion

    }
}
