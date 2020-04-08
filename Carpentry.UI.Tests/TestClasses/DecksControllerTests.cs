//using Carpentry.Data.Models;
using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text;


namespace Carpentry.UI.Tests.TestClasses
{
    [TestClass]
    public class DecksControllerIntegrationTests
    {
        readonly CarpentryFactory _factory;

        public DecksControllerIntegrationTests()
        {
            _factory = new CarpentryFactory();
        }

        //[ClassInitialize]
        //public static void ClassInitialize(TestContext context)
        //{
        //    Console.WriteLine("ClassInitialize");
        //}

        #region Tests - Controller methods all return Ok/Accepted

        [TestMethod]
        public async Task Decks_Add_ReturnsOK_Test()
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
            //Add
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Decks_Update_ReturnsOK_Test()
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
            //Update
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Decks_Delete_ReturnsOK_Test()
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
            //Delete
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Decks_Search_ReturnsOK_Test()
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
            //Search
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Decks_Get_ReturnsOK_Test()
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
            //Get
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Decks_AddCard_ReturnsOK_Test()
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
            //AddCard
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Decks_UpdateCard_ReturnsOK_Test()
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
            //UpdateCard
            Assert.Fail();
        }
        
        [TestMethod]
        public async Task Decks_RemoveCard_ReturnsOK_Test()
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
            //RemoveCard
            Assert.Fail();
        }

        #endregion

        //[TestInitialize]
        //public async Task TestInitialize()
        //{
        //    var client = _factory.WithWebHostBuilder(builder =>
        //    {
        //        builder.ConfigureServices(async services =>
        //        {
        //            var serviceProvider = services.BuildServiceProvider();

        //            using (var scope = serviceProvider.CreateScope())
        //            {
        //                var scopedServices = scope.ServiceProvider;
        //                //var testService = scopedServices.GetRequiredService<TestDataService>();
        //                //await testService.ResetDbAsync();
        //                //await testService.SeedDeckTestRecords();
        //            }
        //        });
        //    })
        //    .CreateClient(new WebApplicationFactoryClientOptions
        //    {
        //        AllowAutoRedirect = false
        //    });

        //}

        //[TestMethod]
        //public void TestsCanIn()
        //{
        //    Assert.IsTrue(true);
        //}

        

    }
}
