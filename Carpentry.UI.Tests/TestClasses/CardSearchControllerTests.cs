using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.TestClasses
{
    public class CardSearchControllerTests
    {
        readonly CarpentryFactory _factory;

        public CardSearchControllerTests()
        {
            _factory = new CarpentryFactory();
        }

        //[TestInitialize]
        //public void TestInitialize()
        //{
        //}

        #region Tests - Controller methods all return Ok/Accepted

        [TestMethod]
        public void CardSearch_SearchWeb_ReturnsOK_Test()
        {
            //SearchWeb
            Assert.Fail();
        }

        [TestMethod]
        public void CardSearch_SearchSet_ReturnsOK_Test()
        {
            //SearchSet
            Assert.Fail();
        }

        [TestMethod]
        public void CardSearch_SearchInventory_ReturnsOK_Test()
        {
            //SearchInventory
            Assert.Fail();
        }

        #endregion

        #region legacy

        //Search by name
        //Verify returns data
        //Verify all results actually match the provided name
        [TestMethod]
        public void Cards_SearchByName_ReturnsValidCards_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Cards_SearchByName_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        //Search by set
        //Verify returns data
        //I guess that all results belong to the expected set
        [TestMethod]
        public void Cards_SearchBySet_ReturnsFullSet_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Cards_SearchBySet_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }


        #endregion

    }
}
