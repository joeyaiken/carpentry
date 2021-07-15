using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    //[TestClass]
    public abstract class CarpentryServiceTestBase
    {



        [TestInitialize]
        public async Task BeforeEach()
        {

            await BeforeEachChild();
        }

        protected abstract Task BeforeEachChild();

        [TestCleanup]
        public async Task AfterEach()
        {

            await AfterEachChild();
        }

        protected abstract Task AfterEachChild();

        protected void ResetContext()
        {

            ResetContextChild();
        }

        protected abstract void ResetContextChild();







    }
}
