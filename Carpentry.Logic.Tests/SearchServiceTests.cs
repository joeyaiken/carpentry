using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class SearchServiceTests : CarpentryServiceTestBase
    {
        [ClassInitialize]
        public async Task BeforeStart()
        {
            //All tests are read-only so the same DB can be used without resetting between tests
            await BeforeEachBase();
        }

        [ClassCleanup]
        public async Task AfterComplete()
        {
            await AfterEachBase();
        }

        protected override bool SeedViews => false;

        protected override void ResetContextChild()
        {
            //throw new NotImplementedException();
        }

        [TestMethod]
        public void SearchServiceTests_AreImplemented_Test()
        {
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public void SearchInventoryCards_SetFilter_Works()
        {

        }



    }
}
