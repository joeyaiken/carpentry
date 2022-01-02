using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpentry.PlaywrightTests.Common.Pages
{
    public class DeckEditorPage : NavigationPage
    {
        public DeckEditorPage(string appUrl, IPage page, AppType appType) : base(page, appType, $"{appUrl}inventory",
            "Inventory")
        {
            
        }

        public async Task WaitForBusy()
        {
            await Page.WaitForSelectorAsync("#progress-bar", new PageWaitForSelectorOptions()
            {
                State = WaitForSelectorState.Hidden
            });
        }

        public async Task ClickAddCardsButton()
        {
            await Page.ClickAsync(".add-cards-button");
        }

        public async Task<int> GetCardCount()
        {
            var cardCountText = await Page.TextContentAsync("#deck-stats-count");
            if (cardCountText == null) return 0;
            return int.TryParse(cardCountText, out var parsedNumber) ? parsedNumber : 0;
        }
        
        public async Task<Dictionary<string, int>> GetTypeToatls()
        {
            var allHeaders = (await Page.QuerySelectorAllAsync(".stats-type-head")).ToList();
            var allValues = await Page.QuerySelectorAllAsync(".stats-type-cell");
            Assert.AreEqual(allHeaders.Count, allValues.Count);

            var result = new Dictionary<string, int>();

            for (var i = 0; i < allHeaders.Count; i++)
            {
                var headerVal = await allHeaders[i].TextContentAsync();
                var cellVal = await allValues[i].TextContentAsync();
                Assert.IsNotNull(headerVal);
                Assert.IsNotNull(cellVal);
                result[headerVal] = int.Parse(cellVal);
            }

            return result;
        }

        public async Task<Dictionary<int, int>> GetCmcTotals()
        {
            var allHeaders = (await Page.QuerySelectorAllAsync(".stats-cmc-head")).ToList();
            var allValues = await Page.QuerySelectorAllAsync(".stats-cmc-cell");
            Assert.AreEqual(allHeaders.Count, allValues.Count);

            var result = new Dictionary<int, int>();

            for (var i = 0; i < allHeaders.Count; i++)
            {
                var headerText = await allHeaders[i].TextContentAsync();
                var cellText = await allValues[i].TextContentAsync();
                Assert.IsNotNull(headerText);
                Assert.IsNotNull(cellText);
                var headerVal = int.Parse(headerText);
                var cellVal = int.Parse(cellText);
                result[headerVal] = cellVal;
            }

            return result;
        }

        public async Task AddCards_SetMinCount(int minCount)
        {
            await SetInputValue("min-count-filter", "1");
        }

        public async Task AddCards_ClickSearch()
        {
            //May have to go by class/id if this fails
            // await Page.ClickAsync("button:text-is(\"Search\")");
            await Page.ClickAsync("#search-button");
            await WaitForBusy();
        }

        public async Task AddCards_ClickRow(string cardName)
        {
            await Page.ClickAsync($".search-result-row:has(td:text-is(\"{cardName}\"))");
            await WaitForBusy();
        }

        // public async Task AddCards_AddInventoryCardByIndex(int rowIndex)
        // {
        //     throw new NotImplementedException();
        // }

        public async Task AddCards_ClickFirstAddCardButton()
        {
            await Page.ClickAsync(".add-inventory-card-button");
            await WaitForBusy();
        }
        
        public async Task AddCards_ClickAddEmpty()
        {
            throw new NotImplementedException();
        }

        public async Task AddCards_ClickClose()
        {
            await Page.ClickAsync("#close-button");
        }


        // public async Task ClickCardRow(string cardName)
        // {
        //     throw new NotImplementedException();
        // }

        public async Task ClickRowDetailButton(string cardName)
        {
            throw new NotImplementedException();
        }

        public async Task CardDetail_ClickClose()
        {
            throw new NotImplementedException();
        }

        public async Task CardDetail_AddInventoryCard(string setCode, int rowIndex)
        {
            //get the row matching the designated set & index
            //click the [...] button
            //click 'Add to Deck'
            //  If an element can't be found, it's expected that some exception gets hit
            throw new NotImplementedException();
        }
        
        
        
    }
}