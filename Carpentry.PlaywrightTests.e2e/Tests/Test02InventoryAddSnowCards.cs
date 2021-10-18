using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e.Pages;
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
        private readonly InventoryAddCardsPage _inventoryAddCardsPage;


        public Test02InventoryAddSnowCards(
            IPage page, 
            string appUrl,
            SeedData seedData, 
            ILogger logger,
            AppType appEnvironment)
        {
            _page = page;
            _appUrl = appUrl;
            _seedData = seedData;
            _logger = logger;
            _inventoryAddCardsPage = new InventoryAddCardsPage(_appUrl, _page, appEnvironment);
        }
        
        public async Task Run()
        {
            await RunSmokeTests();
            await RunSeedTests();
        }

        //Runs a series of tests that don't modify the database
        //Things like adding/removing pending cards, adding special variants
        private async Task RunSmokeTests()
        {
            await TestQuickAdd();
            await TestAddFoil();
            await TestAllButtons();
            await TestCancelClearsMultiple();
        }

        private async Task SmokeTestInit()
        {
            //Navigate to the page, set filters, and search
            await _inventoryAddCardsPage.NavigateTo();
            await _inventoryAddCardsPage.ApplySetFilter("Kaldheim");
            await _inventoryAddCardsPage.ApplySearchGroupFilter("Green");
            await _inventoryAddCardsPage.ClickSearch();
        }
        
        private async Task TestQuickAdd()
        {
            await SmokeTestInit();
            //Get a card
            var cardToQuickAdd = "Blizzard Brawl";
            var cardResult = await _inventoryAddCardsPage.GetSearchResultByName(cardToQuickAdd);
            Assert.IsNotNull(cardResult);
            //quick-add a pending card
            await cardResult.ClickAddButton();
            //confirm is pending
            var card = await _inventoryAddCardsPage.GetPendingCard(cardToQuickAdd);
            Assert.IsNotNull(card);
            Assert.AreEqual(1, await card.GetPendingCount());
            //quick-remove that card
            await cardResult.ClickRemoveButton();
            //confirm 0 pending
            card = await _inventoryAddCardsPage.GetPendingCard(cardToQuickAdd);
            Assert.IsNull(card);
        }

        private async Task TestAddFoil()
        {
            await SmokeTestInit();
            //get the card
            var cardName = "Spirit of the Aldergard";
            var searchResult = await _inventoryAddCardsPage.GetSearchResultByName(cardName);
            Assert.IsNotNull(searchResult);
            await searchResult.Click();
            //add a foil
            var resultCard = (await _inventoryAddCardsPage.GetCardDetails()).Single();
            await resultCard.ClickAddFoil();
            //quick remove
            await searchResult.ClickRemoveButton();
            //confirm 1 pending
            var pendingCards = await _inventoryAddCardsPage.GetAllPendingCards();
            Assert.AreEqual(1, pendingCards.Count);
            //click foil remove
            await resultCard.ClickRemoveFoil();
            //confirm 0 pending
            pendingCards = await _inventoryAddCardsPage.GetAllPendingCards();
            Assert.AreEqual(0, pendingCards.Count);
        }
        
        private async Task TestAllButtons()
        {
            await SmokeTestInit();
            //(of 1 named card...)
            var cardToUse = "Fynn, the Fangbearer";
            var searchResult = await _inventoryAddCardsPage.GetSearchResultByName(cardToUse);
            Assert.IsNotNull(searchResult);
            await searchResult.Click();
            //Assert 2 variants/details
            var resultCards = await _inventoryAddCardsPage.GetCardDetails(); 
            Assert.AreEqual(2, resultCards.Count);
            //add a normal (not quick-add)
            await resultCards[0].ClickAddNormal();
            //Add a foil
            await resultCards[0].ClickAddFoil();
            //Add a variant
            await resultCards[1].ClickAddNormal();
            //add a foil variant
            await resultCards[1].ClickAddFoil();
            //confirm 1 pending ct==4
            var pendingCards = await _inventoryAddCardsPage.GetAllPendingCards();
            Assert.AreEqual(1, pendingCards.Count);
            Assert.AreEqual(4, await pendingCards.Single().GetPendingCount());
            //click quick-remove 4 times
            for (var i = 0; i < 4; i++) await searchResult.ClickRemoveButton();
            //confirm 1 pending ct==3
            pendingCards = await _inventoryAddCardsPage.GetAllPendingCards();
            Assert.AreEqual(1, pendingCards.Count);
            Assert.AreEqual(3, await pendingCards.Single().GetPendingCount());
            //quick-add the normal back
            await searchResult.ClickAddButton();
            //remove all with non-quick button
            await resultCards[0].ClickRemoveNormal();
            await resultCards[0].ClickRemoveFoil();
            await resultCards[1].ClickRemoveNormal();
            await resultCards[1].ClickRemoveFoil();
            //confirm 0 pending
            pendingCards = await _inventoryAddCardsPage.GetAllPendingCards();
            Assert.AreEqual(0, pendingCards.Count);
        }

        private async Task TestCancelClearsMultiple()
        {
            await SmokeTestInit();
            //quick-add 5 different cards (non-linearly)
            var cardsToAdd = new List<string>()
            {
                "Mammoth Growth",
                "Blizzard Brawl",
                "Sculptor of Winter",
                "Boreal Outrider",
                "Fynn, the Fangbearer",
            };
            foreach (var cardName in cardsToAdd)
            {
                var searchResult = await _inventoryAddCardsPage.GetSearchResultByName(cardName);
                Assert.IsNotNull(searchResult);
                await searchResult.ClickAddButton();
            }
            //confirm 5 pending cards, sorted alphabetically
            // var sortedCardNames = cardsToAdd.OrderBy(c => c).ToList();
            var pendingCards = await _inventoryAddCardsPage.GetAllPendingCards();
            Assert.AreEqual(5, pendingCards.Count);
            for (var i = 0; i < 5; i++)
            {
                Assert.AreEqual(cardsToAdd[i], await pendingCards[i].GetCardName());
                Assert.AreEqual(1, await pendingCards[i].GetPendingCount());
            }
            //click cancel
            await _inventoryAddCardsPage.ClickCancel();
            //navigate to the page
            await _inventoryAddCardsPage.NavigateTo();
            //confirm 0 pending
            pendingCards = await _inventoryAddCardsPage.GetAllPendingCards();
            Assert.AreEqual(0, pendingCards.Count);
        }

        private async Task RunSeedTests()
        {
            await _inventoryAddCardsPage.NavigateTo();
            
            foreach (var set in _seedData.SeedSets)
            {
                await _inventoryAddCardsPage.ApplySetFilter(set.SetName);

                foreach (var searchGroup in _seedData.GroupSearchOrder)
                {
                    var searchGroupString = GetSearchGroupString(searchGroup);
                    var cardsInGroup =
                        _seedData.SeedCards.Where(c => c.SetCode == set.SetCode && c.Group == searchGroupString).ToList();
                    if (cardsInGroup.Any())
                    {
                        _logger.Information($"Searching for cards in set: {set.SetName}, group: {searchGroupString}");
                        
                        await _inventoryAddCardsPage.ApplySearchGroupFilter(searchGroupString);
                        await _inventoryAddCardsPage.ClickSearch();
                        foreach (var card in cardsInGroup)
                        {
                            var cardRow = await _inventoryAddCardsPage.GetSearchResultByName(card.CardName);
                            
                            for (var i = 0; i < card.Count; i++)
                                await cardRow.ClickAddButton();
                        }
                    }
                }
            }
            
            //Make assertions about pending cards
            foreach (var card in _seedData.SeedCards)
            {
                // var pendingCardCount = await _inventoryAddCardsPage.GetPendingCardCount(card.CardName);
                var pendingCard = await _inventoryAddCardsPage.GetPendingCard(card.CardName);
                Assert.IsNotNull(pendingCard);
                Assert.AreEqual(card.Count, await pendingCard.GetPendingCount());
            }
            
            //click save
            await _inventoryAddCardsPage.ClickSave();
            
            //wait for things to load
            await Task.Delay(1000);
            Assert.AreEqual($"{_appUrl}inventory", _page.Url);
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