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

        [TestInitialize]
        public async Task BeforeEach()
        {
            await BeforeEachBase();
        }

        [TestCleanup]
        public async Task AfterEach()
        {
            await AfterEachBase();
        }

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
