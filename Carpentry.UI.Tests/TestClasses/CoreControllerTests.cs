using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.TestClasses
{
    public class CoreControllerTests
    {
        //readonly CarpentryFactory _factory;

        public CoreControllerTests()
        {
            //_factory = new CarpentryFactory();
        }

        //[ClassInitialize]
        //public static void ClassInitialize(TestContext context)
        //{
        //    Console.WriteLine("ClassInitialize");
        //}

        //[TestInitialize]
        //public void TestInitialize()
        //{

        //}

        #region Tests - Controller methods all return Ok/Accepted

        [TestMethod]
        public async Task Core_GetFilterValues_ReturnsOK_Test()
        {
            //GetFilterValues
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_BackupDatabase_ReturnsOK_Test()
        {
            //BackupDatabase
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_RestoreDatabase_ReturnsOK_Test()
        {
            //RestoreDatabase
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_GetDatabaseUpdateStatus_ReturnsOK_Test()
        {
            //GetDatabaseUpdateStatus
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_UpdateScryfallSet_ReturnsOK_Test()
        {
            //UpdateScryfallSet
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_UpdateSetData_ReturnsOK_Test()
        {
            //UpdateSetData
            Assert.Fail();
        }

        #endregion

    }
}
