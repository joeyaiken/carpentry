﻿using Carpentry.CarpentryData.Models;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Search;
using Carpentry.Logic.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class SearchServiceTests : CarpentryServiceTestBase
    {

        //service
        private SearchService _searchService = null!;


        [TestInitialize]
        public async Task BeforeStart()
        {
            //All tests are read-only so the same DB can be used without resetting between tests
            await BeforeEachBase();
        }

        [TestCleanup]
        public async Task AfterComplete()
        {
            await AfterEachBase();
        }

        protected override bool SeedViews => false;

        protected override void ResetContextChild()
        {
            _searchService = new SearchService(CardContext);
        }

        [TestMethod]
        public void SearchServiceTests_AreImplemented_Test()
        {
            Assert.Fail("Not implemented");
        }

        //Tests to implement
        //Group (Name/Print/Unique)
        //Filters (set, ...)


        [TestMethod]
        public async Task SearchInventoryCards_GroupBy_Works()
        {
            //seed data


            //can all be 1 set

            //"CardA" #1
            //1 norm
            //1 foil
            //"CardB" #2
            //1 norm
            //"CardA" #? (showcase)
            //1 foil
            //"CardB" #? (extended art)
            //None owned


            ResetContext();
            //query with each group-by option
            //Name
            var byNameQueryParameter = new InventoryQueryParameter()
            {
                GroupBy = nameof(CardSearchGroupBy.name),
            };
            var byNameResult = await _searchService.SearchInventoryCards(byNameQueryParameter);
            //ResetContext();
            //CardId
            var byPrintQueryParameter = new InventoryQueryParameter()
            {
                GroupBy = nameof(CardSearchGroupBy.print),
            };
            var byPrintResult = await _searchService.SearchInventoryCards(byPrintQueryParameter);
            //ResetContext();
            //CardId+IsFoil
            var byUniqueQueryParameter = new InventoryQueryParameter()
            {
                GroupBy = nameof(CardSearchGroupBy.unique),
            };
            var byUniqueResult = await _searchService.SearchInventoryCards(byUniqueQueryParameter);

            //assert on responses




        }

        [TestMethod]
        public async Task SearchInventoryCards_SetFilter_Works()
        {
            //seed DB, 2 records from different sets
            var factory = new CarpentryDataFactory();

            var expectedResultCard = factory.Card("Card A", "R");

            var seedSet1 = factory.CardSet("Set A", "A", DateTime.Now.AddMonths(-3),
                expectedResultCard);

            var seedSet2 = factory.CardSet("Set B", "B", DateTime.Now,
                factory.Card("Card B", "U"));

            CardContext.AddRange(seedSet1, seedSet2);
            await CardContext.SaveChangesAsync();
            
            ResetContext();

            var queryParam = new InventoryQueryParameter()
            {
                GroupBy = nameof(CardSearchGroupBy.print),
                Set = seedSet1.Code,
            };

            //query
            var searchResult = await _searchService.SearchInventoryCards(queryParam);

            //assert on result
            Assert.AreEqual(expectedResultCard.Name, searchResult.Single().Name);
        }

        /// <summary>
        /// A test that checks if the SearchCardDefinitions method generally works, filtering for Set and SearchGroup
        /// </summary>
        [TestMethod]
        public async Task SearchCardDefinitions_Works()
        {
            var factory = new CarpentryDataFactory();

            var redCard = factory.Card("A red one", "R", 'C');
            var blueCard = factory.Card("A blue one", "U", 'C');
            var greenCard = factory.Card("A green one", "G", 'C');

            var expectedSet = factory.CardSet("Set A", "S_A", DateTime.Now, redCard, blueCard);
            var otherSet = factory.CardSet("Set B", "S_B", DateTime.Now.AddMonths(-3), greenCard);
            
            CardContext.AddRange(expectedSet, otherSet);

            await CardContext.SaveChangesAsync();
            ResetContext();
            
            // var cardQuery = await CardContext.Cards.ToListAsync();
                
            var queryParam = new CardSearchQueryParameter()
            {
                Set = expectedSet.Code,
                SearchGroup = nameof(CardSearchGroup.Red),
            };

            var searchResults = await _searchService.SearchCardDefinitions(queryParam);

            Assert.AreEqual(1, searchResults.Count);
        }
    }
}
