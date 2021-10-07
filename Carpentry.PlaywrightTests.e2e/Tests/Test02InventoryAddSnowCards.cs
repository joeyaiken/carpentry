using System;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e.Pages;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace Carpentry.PlaywrightTests.e2e.Tests
{
    public class Test02InventoryAddSnowCards : IRunnableTest
    {
        private readonly IPage _page;
        private readonly string _appUrl;
        private readonly SeedData _seedData;
        private readonly ILogger _logger;

        public Test02InventoryAddSnowCards(IPage page, string appUrl, SeedData seedData, ILogger logger)
        {
            _page = page;
            _appUrl = appUrl;
            _seedData = seedData;
            _logger = logger;
        }
        
        public async Task Run()
        {
            var inventoryAddCardsPage = new InventoryAddCardsPage(_appUrl, _page);
            
            await inventoryAddCardsPage.NavigateTo();
            
            foreach (var set in _seedData.SeedSets)
            {
                await inventoryAddCardsPage.ApplySetFilter(set.SetName);

                foreach (var searchGroup in _seedData.GroupSearchOrder)
                {
                    var searchGroupString = GetSearchGroupString(searchGroup);
                    var cardsInGroup =
                        _seedData.SeedCards.Where(c => c.SetCode == set.SetCode && c.Group == searchGroupString).ToList();
                    if (cardsInGroup.Any())
                    {
                        _logger.Information($"Searching for cards in set: {set.SetName}, group: {searchGroupString}");
                        
                        await inventoryAddCardsPage.ApplySearchGroupFilter(searchGroupString);
                        await inventoryAddCardsPage.ClickSearch();
                        foreach (var card in cardsInGroup)
                        {
                            var cardRow = await inventoryAddCardsPage.GetSearchResultByName(card.CardName);
                            
                            for (var i = 0; i < card.Count; i++)
                                await cardRow.ClickAddButton();
                        }
                    }
                }
            }
            
            //Make assertions about pending cards
            foreach (var card in _seedData.SeedCards)
            {
                var pendingCardCount = await inventoryAddCardsPage.GetPendingCardCount(card.CardName);
                Assert.AreEqual(card.Count, pendingCardCount);
            }
            
            //click save
            await inventoryAddCardsPage.ClickSave();
            
            Assert.AreEqual($"{_appUrl}inventory", _page.Url);

            var inventoryPage = new InventoryPage(_appUrl, _page);
            
            //update filters as desired
            await inventoryPage.SetGroupBy("Name");
            await inventoryPage.SetSortBy("Name");
            await inventoryPage.SetMinValue(1);
            await inventoryPage.SetTakeValue(100);
            //search
            await inventoryPage.ClickSearch();

            //get all card overview objects
            var searchResults = await inventoryPage.GetSearchResults();
            
            //for each seed card, assert it's in the array, then pull from the array
            foreach (var seedCard in _seedData.SeedCards)
            {
                var matchingResult = searchResults.FirstOrDefault(result => result.Name == seedCard.CardName);
                Assert.IsNotNull(matchingResult);
                Assert.AreEqual(seedCard.Count, matchingResult.Count);
                searchResults.Remove(matchingResult);
            }
            
            Assert.AreEqual(0, searchResults.Count);
        }
        
        private static string GetSearchGroupString(CardSearchGroup groupEnum)
        {
            return groupEnum switch
            {
                CardSearchGroup.Red => nameof(CardSearchGroup.Red),
                CardSearchGroup.Blue => nameof(CardSearchGroup.Blue),
                CardSearchGroup.Green => nameof(CardSearchGroup.Green),
                CardSearchGroup.White => nameof(CardSearchGroup.White),
                CardSearchGroup.Black => nameof(CardSearchGroup.Black),
                CardSearchGroup.Multicolored => nameof(CardSearchGroup.Multicolored),
                CardSearchGroup.Colorless => nameof(CardSearchGroup.Colorless),
                CardSearchGroup.Lands => nameof(CardSearchGroup.Lands),
                CardSearchGroup.RareMythic => nameof(CardSearchGroup.RareMythic),
                _ => ""
            };
        }
    }
}