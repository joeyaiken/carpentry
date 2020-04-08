using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.TestClasses
{
    [TestClass]
    public class CoreControllerTests
    {
        readonly CarpentryFactory _factory;

        public CoreControllerTests()
        {
            _factory = new CarpentryFactory();
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
            //assemble
            var client = _factory.CreateClient();

            //act
            //var response = await client.GetAsync("api/CardSearch/");
            //var responseContent = await response.Content.ReadAsStringAsync();

            //assert
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //Assert.AreEqual("Online", responseContent);





            //Add
            Assert.Fail();
            //GetFilterValues
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_BackupDatabase_ReturnsOK_Test()
        {
            //assemble
            var client = _factory.CreateClient();

            //act
            //var response = await client.GetAsync("api/CardSearch/");
            //var responseContent = await response.Content.ReadAsStringAsync();

            //assert
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //Assert.AreEqual("Online", responseContent);





            //Add
            Assert.Fail();
            //BackupDatabase
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_RestoreDatabase_ReturnsOK_Test()
        {
            //assemble
            var client = _factory.CreateClient();

            //act
            //var response = await client.GetAsync("api/CardSearch/");
            //var responseContent = await response.Content.ReadAsStringAsync();

            //assert
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //Assert.AreEqual("Online", responseContent);





            //Add
            Assert.Fail();
            //RestoreDatabase
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_GetDatabaseUpdateStatus_ReturnsOK_Test()
        {
            //assemble
            var client = _factory.CreateClient();

            //act
            //var response = await client.GetAsync("api/CardSearch/");
            //var responseContent = await response.Content.ReadAsStringAsync();

            //assert
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //Assert.AreEqual("Online", responseContent);





            //Add
            Assert.Fail();
            //GetDatabaseUpdateStatus
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_UpdateScryfallSet_ReturnsOK_Test()
        {
            //assemble
            var client = _factory.CreateClient();

            //act
            //var response = await client.GetAsync("api/CardSearch/");
            //var responseContent = await response.Content.ReadAsStringAsync();

            //assert
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //Assert.AreEqual("Online", responseContent);





            //Add
            Assert.Fail();
            //UpdateScryfallSet
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Core_UpdateSetData_ReturnsOK_Test()
        {
            //assemble
            var client = _factory.CreateClient();

            //act
            //var response = await client.GetAsync("api/CardSearch/");
            //var responseContent = await response.Content.ReadAsStringAsync();

            //assert
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //Assert.AreEqual("Online", responseContent);





            //Add
            Assert.Fail();
            //UpdateSetData
            Assert.Fail();
        }

        #endregion

    }
}
