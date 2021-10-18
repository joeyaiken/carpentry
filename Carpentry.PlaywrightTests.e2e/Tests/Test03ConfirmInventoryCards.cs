using System.Linq;
using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e.Pages;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpentry.PlaywrightTests.e2e.Tests
{
    public class Test03ConfirmInventoryCards : IRunnableTest
    {
        private readonly IPage _page;
        private readonly string _appUrl;
        private readonly SeedData _seedData;
        private readonly AppType _appEnvironment;

        public Test03ConfirmInventoryCards(IPage page, string appUrl, SeedData seedData, AppType appEnvironment)
        {
            _page = page;
            _appUrl = appUrl;
            _seedData = seedData;
            _appEnvironment = appEnvironment;
        }
        
        public async Task Run()
        {
            var inventoryPage = new InventoryPage(_appUrl, _page, _appEnvironment);
            await inventoryPage.NavigateTo();
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
                var matchingResult = searchResults.FirstOrDefault(result => result.GetName().Result == seedCard.CardName);
                Assert.IsNotNull(matchingResult);
                Assert.AreEqual(seedCard.Count, matchingResult.GetTotal().Result);
                searchResults.Remove(matchingResult);
            }
            
            Assert.AreEqual(0, searchResults.Count);
        }
    }
}