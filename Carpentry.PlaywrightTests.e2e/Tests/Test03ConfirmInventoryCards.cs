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
        private readonly AppSettings _appSettings;
        private readonly SeedData _seedData;
        public Test03ConfirmInventoryCards(IPage page, IOptions<AppSettings> appSettings, SeedData seedData)
        {
            _page = page;
            _appSettings = appSettings.Value;
            _seedData = seedData;
        }
        
        public async Task Run()
        {
            var inventoryPage = new InventoryPage(_appSettings.AppUrl, _page);
            
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
    }
}