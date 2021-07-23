using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class DataUpdateServiceTests : CarpentryServiceTestBase
    {
        protected override bool SeedViews => false;
        protected override Task BeforeEachChild() 
        {
            var mockLogger = new Mock<ILogger<DataUpdateService>>();
            var mockScryService = new Mock<IScryfallService>();
            var mockIntegrityService = new Mock<IDataIntegrityService>();

            _updateService = new DataUpdateService(
                mockLogger.Object,
                mockScryService.Object,
                mockIntegrityService.Object,
                CardContext);

            return Task.CompletedTask;
        }


        protected override Task AfterEachChild() => Task.CompletedTask;


        private DataUpdateService _updateService = null!;

        protected override void ResetContextChild()
        {
            //throw new NotImplementedException();
        }

        ////Task ValidateDatabase();
        //[TestMethod]
        //public async Task ValidateDatabase_Works_Test()
        //{
        //    //do I really care about testing this?...
        //}

        //Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update);
        [TestMethod]
        public async Task GetTrackedSets_Works_Test()
        {
            Assert.Fail("Not implemented");
        }

        //Task AddTrackedSet(int setId);
        [TestMethod]
        public async Task AddTrackedSet_Works_Test()
        {
            Assert.Fail("Not implemented");
        }

        //Task RemoveTrackedSet(int setId);
        [TestMethod]
        public async Task RemoveTrackedSet_Works_Test()
        {
            Assert.Fail("Not implemented");
        }

        //Task UpdateTrackedSet(int setId);
        [TestMethod]
        public async Task UpdateTrackedSet_Works_Test()
        {
            Assert.Fail("Not implemented");
        }

        //Task TryUpdateAvailableSets();
        [TestMethod]
        public async Task TryUpdateAvailableSets_Works_Test()
        {

            //cards DB starts with 0 sets



            //call method
            await _updateService.TryUpdateAvailableSets();

            //confirm 


        }
    }
}
