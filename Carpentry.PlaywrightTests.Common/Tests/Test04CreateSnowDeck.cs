using System.Threading.Tasks;
using Carpentry.PlaywrightTests.Common.Pages;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpentry.PlaywrightTests.Common.Tests
{
    public class Test04CreateSnowDeck : IRunnableTest
    {
        private readonly IPage _page;
        private readonly string _appUrl;
        private readonly SeedData _seedData;
        private readonly AppType _appType;

        public Test04CreateSnowDeck(IPage page, string appUrl, SeedData seedData, AppType appType)
        {
            _page = page;
            _appUrl = appUrl;
            _seedData = seedData;
            _appType = appType;
        }
        
        public async Task Run()
        {
            var homePage = new HomePage(_appUrl, _page, _appType);
            await homePage.NavigateTo();
            
            // Create a new empty deck
            await homePage.ClickAddDeck();
            await homePage.SetNewDeckTitle(_seedData.SeedSnowDeck.DeckName);
            await homePage.SetNewDeckFormat(_seedData.SeedSnowDeck.DeckFormat);
            await homePage.SetNewDeckNotes(_seedData.SeedSnowDeck.DeckNotes);
            await homePage.ClickSaveNewDeck();
            
            var deckEditorPage = new DeckEditorPage(_appUrl, _page, _appType);
            await deckEditorPage.WaitForBusy();
            
            // Add all cards from seed data, and populate from inventory
            await deckEditorPage.ClickAddCardsButton();
            await deckEditorPage.AddCards_SetMinCount(1);
            await deckEditorPage.AddCards_ClickSearch();
            foreach (var seedCard in _seedData.SeedCards)
            {
                await deckEditorPage.AddCards_ClickRow(seedCard.CardName);
                for (var i = 0; i < seedCard.Count; i++)
                    await deckEditorPage.AddCards_ClickFirstAddCardButton();
            }
            await deckEditorPage.AddCards_ClickClose();
            
            // Make assertions about deck editor cards & statuses (stars)
            
            // Assert card count == 60
            var cardCount = await deckEditorPage.GetCardCount();
            Assert.AreEqual(_seedData.SeedSnowDeck.ExpectedCardCount, cardCount);
            
            // Assert # of type totals
            var typeBreakdown = await deckEditorPage.GetTypeToatls();
            foreach (var key in _seedData.SeedSnowDeck.ExpectedTypeBreakdown.Keys)
            {
                Assert.IsTrue(typeBreakdown.TryGetValue(key, out var value));
                Assert.AreEqual(_seedData.SeedSnowDeck.ExpectedTypeBreakdown[key], value);
            }
            
            // Assert curve
            var cmcTotals = await deckEditorPage.GetCmcTotals();
            foreach (var key in _seedData.SeedSnowDeck.ExpectedCurveBreakdown.Keys)
            {
                Assert.IsNotNull(cmcTotals.TryGetValue(key, out var value));
                Assert.AreEqual(_seedData.SeedSnowDeck.ExpectedCurveBreakdown, value);
            }
            
            //return home
            await homePage.NavigateTo();
            
            // Get the deck in the deck list, then assert validity
            var seedDeck = await homePage.GetDeckByName(_seedData.SeedSnowDeck.DeckName);
            var deckValidity = await seedDeck.GetValidity();
            Assert.AreEqual(_seedData.SeedSnowDeck.ExpectedValidity, deckValidity);
            
            // Assert colors are actually UG and not empty
            var deckColors = await seedDeck.GetDeckColors();
            Assert.AreEqual(_seedData.SeedSnowDeck.ExpectedColors, deckColors);
        }
    }
}