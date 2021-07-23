﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class DataIntegrityServiceTests : CarpentryServiceTestBase
    {
        protected override Task BeforeEachChild() => Task.CompletedTask;
        protected override Task AfterEachChild() => Task.CompletedTask;
        protected override bool SeedViews => false;

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