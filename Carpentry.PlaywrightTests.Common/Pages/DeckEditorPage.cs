using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace Carpentry.PlaywrightTests.Common.Pages
{
    public class DeckEditorPage : NavigationPage
    {
        public DeckEditorPage(string appUrl, IPage page, AppType appType) : base(page, appType, $"{appUrl}inventory", "Inventory")
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
            throw new NotImplementedException();
        }
        
        public async Task<Dictionary<string, int>> GetTypeToatls()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<int, int>> GetCmcTotals()
        {
            throw new NotImplementedException();
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