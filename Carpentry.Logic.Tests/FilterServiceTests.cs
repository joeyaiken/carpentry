﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class FilterServiceTests : CarpentryServiceTestBase
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

        protected override void ResetContextChild()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DataImportServiceTests_AreImplemented_Test()
        {
            Assert.Fail("Not implemented");
        }
    }
}
