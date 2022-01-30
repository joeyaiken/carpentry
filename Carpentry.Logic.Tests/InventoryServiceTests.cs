using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class InventoryServiceTests : CarpentryServiceTestBase
    {
        protected override bool SeedViews => false;
        private InventoryService _trimmingToolService = null!;
        private Mock<ILogger<InventoryService>> _mockLogger = null!;

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
            _mockLogger = new Mock<ILogger<InventoryService>>();
            _trimmingToolService = new InventoryService(CardContext, _mockLogger.Object);//context, logger
        }

        [TestMethod]
        public void InventoryServiceTests_AreImplemented_Test()
        {
            Assert.Fail("Not implemented");
        }




        [TestMethod]
        public async Task GetCollectionTotals_WorksWithNoCards()
        {
            //don't want any data seeded
            var totals = await _trimmingToolService.GetCollectionTotals();

            Assert.AreEqual(3, totals.Count);
        }


        [TestMethod]
        public async Task GetCollectionTotals_AnotherOne()
        {

            var cardSet = new CardSetData()
            {
                Name = "ASet",
                Code = "SET",
                IsTracked = true,
                LastUpdated = DateTime.Now,
                ReleaseDate = DateTime.Now,
                Cards = new List<CardData>()
                {
                    new CardData()
                    {
                        RarityId = 'R',
                        Name = "ACardName",
                        Price = 1,
                        PriceFoil = 100,
                        InventoryCards = new List<InventoryCardData>()
                        {
                            new InventoryCardData()
                            {
                                InventoryCardStatusId = 1,
                                IsFoil = false,
                            },
                            new InventoryCardData()
                            {
                                InventoryCardStatusId = 1,
                                IsFoil = true,
                            },

                            new InventoryCardData()
                            {
                                InventoryCardStatusId = 2,
                                IsFoil = false,
                            },
                            new InventoryCardData()
                            {
                                InventoryCardStatusId = 2,
                                IsFoil = true,
                            },
                            
                            new InventoryCardData()
                            {
                                InventoryCardStatusId = 3,
                                IsFoil = false,
                            },
                            new InventoryCardData()
                            {
                                InventoryCardStatusId = 3,
                                IsFoil = true,
                            }
                        }
                    }
                },
            };

            CardContext.Add(cardSet);
            await CardContext.SaveChangesAsync();
            ResetContext();


            //don't want any data seeded
            var totals = await _trimmingToolService.GetCollectionTotals();

            Assert.AreEqual(3, totals.Count);
        }

    }
}
