//using Carpentry.Data.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Carpentry.UI.Tests.IntegrationTests
{
    //[TestClass]
    public class InventoryControllerIntegrationTests
    {
        readonly CarpentryFactory _factory;
        readonly HttpClient _client;

        public InventoryControllerIntegrationTests()
        {
            _factory = new CarpentryFactory();
            _client = _factory.CreateClient();
        }

        //#region Tests - Controller methods all return Ok/Accepted

        //[TestMethod]
        //public async Task Inventory_Add_ReturnsOK_Test()
        //{
        //    //assemble
        //    var client = _factory.CreateClient();

        //    //act
        //    //var response = await client.GetAsync("api/CardSearch/");
        //    //var responseContent = await response.Content.ReadAsStringAsync();

        //    //assert
        //    //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //    //Assert.AreEqual("Online", responseContent);





        //    //Add
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public async Task Inventory_AddBatch_ReturnsOK_Test()
        //{
        //    //assemble
        //    var client = _factory.CreateClient();

        //    //act
        //    //var response = await client.GetAsync("api/CardSearch/");
        //    //var responseContent = await response.Content.ReadAsStringAsync();

        //    //assert
        //    //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //    //Assert.AreEqual("Online", responseContent);





        //    //Add
        //    Assert.Fail();
        //    //AddBatch
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public async Task Inventory_Update_ReturnsOK_Test()
        //{
        //    //assemble
        //    var client = _factory.CreateClient();

        //    //act
        //    //var response = await client.GetAsync("api/CardSearch/");
        //    //var responseContent = await response.Content.ReadAsStringAsync();

        //    //assert
        //    //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //    //Assert.AreEqual("Online", responseContent);





        //    //Add
        //    Assert.Fail();
        //    //Update
        //    Assert.Fail();
        //}
        
        //[TestMethod]
        //public async Task Inventory_Delete_ReturnsOK_Test()
        //{
        //    //assemble
        //    var client = _factory.CreateClient();

        //    //act
        //    //var response = await client.GetAsync("api/CardSearch/");
        //    //var responseContent = await response.Content.ReadAsStringAsync();

        //    //assert
        //    //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //    //Assert.AreEqual("Online", responseContent);





        //    //Add
        //    Assert.Fail();
        //    //Delete
        //    Assert.Fail();
        //}
        ////Delete

        //[TestMethod]
        //public async Task Inventory_Search_ReturnsOK_Test()
        //{
        //    //assemble
        //    var client = _factory.CreateClient();

        //    //act
        //    //var response = await client.GetAsync("api/CardSearch/");
        //    //var responseContent = await response.Content.ReadAsStringAsync();

        //    //assert
        //    //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //    //Assert.AreEqual("Online", responseContent);





        //    //Add
        //    Assert.Fail();
        //    //Search
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public async Task Inventory_GetByName_ReturnsOK_Test()
        //{
        //    //assemble
        //    var client = _factory.CreateClient();

        //    //act
        //    //var response = await client.GetAsync("api/CardSearch/");
        //    //var responseContent = await response.Content.ReadAsStringAsync();

        //    //assert
        //    //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //    //Assert.AreEqual("Online", responseContent);





        //    //Add
        //    Assert.Fail();
        //    //GetByName
        //    Assert.Fail();
        //}

        //#endregion

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
        //                var testService = scopedServices.GetRequiredService<TestDataService>();
        //                await testService.ResetDbAsync();
        //                //await testService.SeedDeckTestRecords();
        //            }
        //        });
        //    })
        //    .CreateClient(new WebApplicationFactoryClientOptions
        //    {
        //        AllowAutoRedirect = false
        //    });

        //}

        
    }
}
