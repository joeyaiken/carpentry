using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Tests.TestClasses
{
    /// <summary>
    /// The Cards Controller should probably be renamed since all this does is interact with the scryfall repo
    /// </summary>
    [TestClass]
    public class CardsControllerIntegrationTests
    {
        readonly CarpentryFactory _factory;

        public CardsControllerIntegrationTests()
        {
            _factory = new CarpentryFactory();
        }

        [TestInitialize]
        public void TestInitialize()
        {

        }

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



    }
}
