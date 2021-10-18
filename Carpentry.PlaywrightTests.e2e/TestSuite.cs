using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e.Pages;
using Carpentry.PlaywrightTests.e2e.Tests;
using Serilog;

namespace Carpentry.PlaywrightTests.e2e
{
    
    
    /// <summary>
    /// An end-to-end test suite.  Runs the full carpentry e2e test suite that should be run against both apps
    /// Eventually test-logic will be refactored out to a location where it can also be called by unit tests
    /// (Alternatively, unit tests could call the tests directly)
    /// </summary>
    public class TestSuite
    {
        // private readonly IOptions<AppSettings> _appSettings;
        private readonly AppSettings _appSettings;
        private static int APP_INIT_DELAY = 5000;
        private readonly SeedData _seedData;
        private readonly ILogger _logger;
        // private readonly string APP_URL;
        

        public TestSuite(
            // IOptions<AppSettings> appSettings,
            AppSettings appSettings,
            ILogger logger
            )
        {
            _appSettings = appSettings;
            _seedData = new SeedData();
            _logger = logger;
            // APP_URL = appSettings.Value.ReactUrl;
            // APP_URL = appSettings.Value.AngularUrl;
        }

        public async Task RunTestSuite()
        {
            _logger.Information("Beginning Playwright e2e test for Carpentry app");

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false,
                //SlowMo = 50,
            });

            //Wait for the app to load

            //Verify app is using a test DB

            //Reset/seed the DB (alternatively, ensure the app is/was seeded on init)

            //Run the desired test(s)

            //Test that the home page loads properly

            //Build the 'Simic Snow Stompy' deck by adding tracked sets, adding new inventory cards, then building a deck from those cards

            //Build John Snow by importing a deck list

            //Build [Some other new deck] by creating new cards directly into to a deck

            //[find some way to get bulk cards], then use Trimming Tool to trim cards

            //TODO : Ensure all features are covered by e2e tests

            //await DemoTest();
            //return;

            var page = await browser.NewPageAsync();

            await WaitForApp(page);

            //TODO - Find an appropriate way to load these with DI instead of constructing them manually (or something more graceful)
            var synchronousRunnables = new List<IRunnableTest>()
            {
                new Test00HomePageLoads(page, _appSettings.AppUrl, _logger),
                // new Test01SettingsTrackSnowSets(page, _appSettings.AppUrl, _logger),
                // new Test02InventoryAddSnowCards(page, _appSettings.AppUrl, _seedData, _logger, _appSettings.AppEnvironment),
                new Test03ConfirmInventoryCards(page, _appSettings.AppUrl, _seedData),
            };

            foreach (var runnable in synchronousRunnables)
            {
                await runnable.Run();
            }
            
            //Add the cards required to populate the snow deck
            // await TestInventoryAddSnowCards(page);

            _logger.Information("Finished running test suite");
        }

        private async Task TestInventoryAddSnowCards(IPage page)
        {
            var inventoryAddCardsPage = new InventoryAddCardsPage(_appSettings.AppUrl, page, _appSettings.AppEnvironment);
            
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
                // var pendingCardCount = await inventoryAddCardsPage.GetPendingCardCount(card.CardName);
                var pendingCard = await inventoryAddCardsPage.GetPendingCard(card.CardName);
                Assert.IsNotNull(pendingCard);
                Assert.AreEqual(card.Count, await pendingCard.GetPendingCount());
            }
            
            //click save
            await inventoryAddCardsPage.ClickSave();
            
            Assert.AreEqual($"{_appSettings.AppUrl}inventory", page.Url);

            var inventoryPage = new InventoryPage(_appSettings.AppUrl, page);
            
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
        
        private async Task DemoTest() 
        {
            _logger.Information("Beginning Playwright e2e test for Carpentry app");

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false,
                SlowMo = 500,
            });
            var page = await browser.NewPageAsync();


            await page.GotoAsync("https://playwright.dev/dotnet");

            //await page.TextContentAsync(".header > .h1")
            var headerText = await page.TextContentAsync("header h1");

            Assert.AreEqual("Playwright enables reliable end-to-end testing for modern web apps.", headerText);

            _logger.Information("Test executed successfully");
        }

        private async Task WaitForApp(IPage page)
        {
            _logger.Information($"Beginning {nameof(WaitForApp)}");
            //bool pageIsLoaded = await TryLoadPage(page);
            bool pageIsLoaded = false;
            while (!pageIsLoaded)
            {
                // pageIsLoaded = await TryLoadPage(page);
                //TODO - implement some limit/timeout
                //Either this properly awaits the page, or fails and should delay
                try //TODO - catch specific errors, don't catch blindly
                {
                    _logger.Information("TryLoadPage");
                    await page.GotoAsync(_appSettings.AppUrl, new PageGotoOptions() { Timeout = 0 }); //TODO add a natural timeout instead of handling an exception
                    _logger.Information("Page Loaded");
                    //return true;
                    pageIsLoaded = true;
                }
                catch //(PlaywrightException ex)
                {
                    //_logger.Information($"Page Not loaded: {ex.Message}");
                    _logger.Information($"Error loading page, delaying before retrying...");
                    await Task.Delay(APP_INIT_DELAY);
                    //return false;
                }
            }

            //bool appIsLoaded = await TryLoadApp(page);
            //bool appIsLoaded = !pageIsLoaded ? false : await TryLoadApp(page);
            bool appIsLoaded = false;
            //while (!(await AppIsLoaded(page)))
            //while (!appIsLoaded)
            //{
            //    if (!pageIsLoaded) pageIsLoaded = await TryLoadPage(page);
            //    if (pageIsLoaded) appIsLoaded = await TryLoadApp(page);
            //    _logger.Information("App not loaded, retrying soon...");
            //    await Task.Delay(5000);
            //}

            while (!appIsLoaded)
            {
                //appIsLoaded = await TryLoadApp(page);
                try //TODO - catch specific errors, don't catch blindly
                {
                    _logger.Information("Checking app status (TryLoadApp)");
                    var appText = await page.TextContentAsync("#root");
                    if (appText != "Loading...")
                    {
                        _logger.Information("App loaded!");
                        appIsLoaded = true;
                        continue;
                    };
                }
                catch
                {
                    _logger.Information("Error loading app");
                }

                _logger.Information("App not loaded, delaying before retrying...");
                await Task.Delay(APP_INIT_DELAY);
            }
            _logger.Information($"{nameof(WaitForApp)} completed");
        }

        //private async Task<bool> TryLoadPage(IPage page)
        //{
        //    //TODO - implement some limit/timeout
        //    //Either this properly awaits the page, or fails and should delay
        //    try //TODO - catch specific errors, don't catch blindly
        //    {
        //        _logger.Information("TryLoadPage");
        //        await page.GotoAsync(_appSettings.AppUrl, new PageGotoOptions() { Timeout = 0 }); //TODO add a natural timeout instead of handling an exception
        //        _logger.Information("Page Loaded");
        //        return true;
        //    }
        //    catch //(PlaywrightException ex)
        //    {
        //        //_logger.Information($"Page Not loaded: {ex.Message}");
        //        _logger.Information($"Error loading page, delaying before retrying...");
        //        await Task.Delay(APP_INIT_DELAY);
        //        return false;
        //    }
        //}

        //private async Task<bool> TryLoadApp(IPage page)
        //{
        //    try //TODO - catch specific errors, don't catch blindly
        //    {
        //        _logger.Information("Checking app status (TryLoadApp)");
        //        //await page.GotoAsync(APP_URL);
        //        var appText = await page.TextContentAsync("app-root");
        //        if (appText != "Loading...")
        //        {
        //            _logger.Information("App loaded!");
        //            return true;
        //        };
        //    }
        //    catch
        //    {
        //        _logger.Information("Error loading app");
        //    }

        //    _logger.Information("App not loaded, delaying before retrying...");
        //    await Task.Delay(APP_INIT_DELAY);
        //    return false;

        //}

    }

    public class SeedData
    {
        public SeedData()
        {
            KaldheimSet = new SeedSet("khm", "Kaldheim");
            ModernHorizonsSet = new SeedSet("mh1", "Modern Horizons");
            ColdsnapSet = new SeedSet("csp", "Coldsnap");

            SeedCards = new List<SeedCard>()
            {
                new SeedCard("Ascendant Spirit", KaldheimSet.SetCode, nameof(CardSearchGroup.RareMythic), 4),
                new SeedCard("Frost Augur", KaldheimSet.SetCode, nameof(CardSearchGroup.Blue), 4),
                
                new SeedCard("Blizzard Brawl", KaldheimSet.SetCode, nameof(CardSearchGroup.Green), 4),
                
                new SeedCard("The Three Seasons", KaldheimSet.SetCode, nameof(CardSearchGroup.Multicolored), 2),
                
                new SeedCard("Faceless Haven", KaldheimSet.SetCode, nameof(CardSearchGroup.RareMythic), 2),
                new SeedCard("Rimewood Falls", KaldheimSet.SetCode, nameof(CardSearchGroup.Lands), 4),

                new SeedCard("Marit Lage's Slumber", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.RareMythic), 4),
                
                new SeedCard("Conifer Wurm", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Green), 4),
                new SeedCard("Glacial Revelation", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Green), 4),
                
                new SeedCard("Ice-Fang Coatl", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.RareMythic), 4),
                new SeedCard("Abominable Treefolk", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Multicolored), 4),
                
                new SeedCard("Snow-Covered Forest", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Lands), 8),
                new SeedCard("Snow-Covered Island", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Lands), 8),

                new SeedCard("Boreal Druid", ColdsnapSet.SetCode, nameof(CardSearchGroup.Green), 4),
            };

            GroupSearchOrder = new List<CardSearchGroup>()
            {
                CardSearchGroup.Blue,
                CardSearchGroup.Green,
                CardSearchGroup.Multicolored,
                CardSearchGroup.Lands,
                CardSearchGroup.RareMythic,
            };
        }

        public SeedSet ColdsnapSet { get; set; }
        public SeedSet ModernHorizonsSet { get; set; }
        public SeedSet KaldheimSet { get; set;  }
        
        public List<SeedCard> SeedCards { get; }
        
        public List<CardSearchGroup> GroupSearchOrder { get; }

        public List<SeedSet> SeedSets => new List<SeedSet>()
        {
            KaldheimSet, ModernHorizonsSet, ColdsnapSet
        };
        
        public List<string> SeedSetCodes { get; }
    }

    public class SeedSet
    {
        public SeedSet(string setCode, string setName)
        {
            SetCode = setCode;
            SetName = setName;
        }
        
        public string SetCode { get; set; }
        public string SetName { get; set; }
    }

    public class SeedCard
    {
        public SeedCard(string cardName, string setCode, string group, int count)
        {
            SetCode = setCode;
            Group = group;
            CardName = cardName;
            Count = count;
        }
        public string SetCode { get; }
        public string Group { get; }
        public string CardName { get; }
        public int Count { get; }
    }

    // public enum CardGroup
    // {
    //     U,
    //     G,
    //     Multi,
    //     Land,
    // }

    public enum SetCodes
    {
        khm,
        mh1,
        csp
    }
}