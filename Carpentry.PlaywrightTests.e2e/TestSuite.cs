using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Logic.Models;
using Carpentry.PlaywrightTests.e2e.Pages;

namespace Carpentry.PlaywrightTests.e2e
{
    /// <summary>
    /// An end-to-end test suite.  Runs the full carpentry e2e test suite that should be run against both apps
    /// Eventually test-logic will be refactored out to a location where it can also be called by unit tests
    /// (Alternatively, unit tests could call the tests directly)
    /// </summary>
    public class TestSuite
    {
        private readonly AppSettings _appSettings;
        private static int APP_INIT_DELAY = 5000;
        private readonly SeedData _seedData;

        public TestSuite(
            IOptions<AppSettings> appSettings
            )
        {
            _appSettings = appSettings.Value;
            _seedData = new SeedData();
        }

        public async Task RunTestSuite()
        {
            Console.WriteLine("Beginning Playwright e2e test for Carpentry app");

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

            //await TestHomePageLoads(page);

            // //navigate to settings, add desired tracked sets (the 3 needed for SimicSnowStompy)
            // await TestSettingsTrackSnowSets(page);

            //Add the cards required to populate the snow deck
            await TestInventoryAddSnowCards(page);

        }



        private async Task TestHomePageLoads(IPage page)
        {
            Console.WriteLine($"starting {nameof(TestHomePageLoads)}");

            var homePage = new HomePage(_appSettings.AppUrl, page);

            await homePage.NavigateTo();

            Assert.AreEqual("Carpentry", homePage.TitleText);

            Assert.AreEqual("A deck & inventory management tool for Magic the Gathering", homePage.SubtitleText);

            //TODO - Make assertions about the deck list
            Console.WriteLine($"{nameof(TestHomePageLoads)} completed successfully");
        }

        private async Task TestSettingsTrackSnowSets(IPage page)
        {
            var settingsPage = new SettingsPage(_appSettings.AppUrl, page);
            await settingsPage.NavigateTo();
            await settingsPage.WaitForBusy();
            
            //Assert no tracked sets
            var trackedSets = await settingsPage.GetTrackedSetRows();
            Assert.AreEqual(0, trackedSets.Count);
            
            //show untracked sets
            await settingsPage.ClickShowUntrackedToggle();
            
            //refresh list
            await settingsPage.ClickRefreshButton();

            //add Kaldheim, MH1, and Coldsnap
            await settingsPage.AddTrackedSet(nameof(SetCodes.khm));
            await settingsPage.AddTrackedSet(nameof(SetCodes.mh1));
            await settingsPage.AddTrackedSet(nameof(SetCodes.csp));

            //Hide untracked sets
            await settingsPage.ClickShowUntrackedToggle();

            //Assert 3 tracked sets (& details ?)
            var updatedTrackedSets = await settingsPage.GetTrackedSetRows();
            Assert.AreEqual(3, updatedTrackedSets.Count);

            Assert.IsNotNull(await settingsPage.GetRowByCode(nameof(SetCodes.khm)));
            Assert.IsNotNull(await settingsPage.GetRowByCode(nameof(SetCodes.mh1)));
            Assert.IsNotNull(await settingsPage.GetRowByCode(nameof(SetCodes.csp)));
        }

        private async Task TestInventoryAddSnowCards(IPage page)
        {
            var inventoryAddCardsPage = new InventoryAddCardsPage(_appSettings.AppUrl, page);
            
            //navigate to Inventory
            //click Add Cards
            await inventoryAddCardsPage.NavigateTo();
            
            //for each set
            //  Search for a color group
            //  add all cards
            //  repeat for all color groups, and all 3 sets
            
            //POC - search for KHM, Blue
            
            
            //POC - Click 1 card once, cick another card twice
            //POC - Confirm pending cards
            
            
            //add cards by set
            //MH1
            //Kaldheim
            //Coldsnap

            int breakpoint = 1;
        }
        
        private async Task DemoTest() 
        {
            Console.WriteLine("Beginning Playwright e2e test for Carpentry app");

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

            Console.WriteLine("Test executed successfully");
        }

        private async Task WaitForApp(IPage page)
        {
            Console.WriteLine($"Beginning {nameof(WaitForApp)}");
            //bool pageIsLoaded = await TryLoadPage(page);
            bool pageIsLoaded = false;
            while (!pageIsLoaded)
            {
                // pageIsLoaded = await TryLoadPage(page);
                //TODO - implement some limit/timeout
                //Either this properly awaits the page, or fails and should delay
                try //TODO - catch specific errors, don't catch blindly
                {
                    Console.WriteLine("TryLoadPage");
                    await page.GotoAsync(_appSettings.AppUrl, new PageGotoOptions() { Timeout = 0 }); //TODO add a natural timeout instead of handling an exception
                    Console.WriteLine("Page Loaded");
                    //return true;
                    pageIsLoaded = true;
                }
                catch //(PlaywrightException ex)
                {
                    //Console.WriteLine($"Page Not loaded: {ex.Message}");
                    Console.WriteLine($"Error loading page, delaying before retrying...");
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
            //    Console.WriteLine("App not loaded, retrying soon...");
            //    await Task.Delay(5000);
            //}

            while (!appIsLoaded)
            {
                //appIsLoaded = await TryLoadApp(page);
                try //TODO - catch specific errors, don't catch blindly
                {
                    Console.WriteLine("Checking app status (TryLoadApp)");
                    //await page.GotoAsync(APP_URL);
                    var appText = await page.TextContentAsync("app-root");
                    if (appText != "Loading...")
                    {
                        Console.WriteLine("App loaded!");
                        appIsLoaded = true;
                        continue;
                    };
                }
                catch
                {
                    Console.WriteLine("Error loading app");
                }

                Console.WriteLine("App not loaded, delaying before retrying...");
                await Task.Delay(APP_INIT_DELAY);
                //return false;
            }
            Console.WriteLine($"{nameof(WaitForApp)} completed");
        }

        //private async Task<bool> TryLoadPage(IPage page)
        //{
        //    //TODO - implement some limit/timeout
        //    //Either this properly awaits the page, or fails and should delay
        //    try //TODO - catch specific errors, don't catch blindly
        //    {
        //        Console.WriteLine("TryLoadPage");
        //        await page.GotoAsync(_appSettings.AppUrl, new PageGotoOptions() { Timeout = 0 }); //TODO add a natural timeout instead of handling an exception
        //        Console.WriteLine("Page Loaded");
        //        return true;
        //    }
        //    catch //(PlaywrightException ex)
        //    {
        //        //Console.WriteLine($"Page Not loaded: {ex.Message}");
        //        Console.WriteLine($"Error loading page, delaying before retrying...");
        //        await Task.Delay(APP_INIT_DELAY);
        //        return false;
        //    }
        //}

        //private async Task<bool> TryLoadApp(IPage page)
        //{
        //    try //TODO - catch specific errors, don't catch blindly
        //    {
        //        Console.WriteLine("Checking app status (TryLoadApp)");
        //        //await page.GotoAsync(APP_URL);
        //        var appText = await page.TextContentAsync("app-root");
        //        if (appText != "Loading...")
        //        {
        //            Console.WriteLine("App loaded!");
        //            return true;
        //        };
        //    }
        //    catch
        //    {
        //        Console.WriteLine("Error loading app");
        //    }

        //    Console.WriteLine("App not loaded, delaying before retrying...");
        //    await Task.Delay(APP_INIT_DELAY);
        //    return false;

        //}

    }

    public class SeedData
    {
        public SeedData()
        {
            KaldheimSet = new SeedSet("khm");
            ModernHorizonsSet = new SeedSet("mh1");
            ColdsnapSet = new SeedSet("csp");

            SeedCards = new List<SeedCard>()
            {
                new SeedCard("Ascendant Spirit", KaldheimSet.SetCode, nameof(CardSearchGroup.RareMythic), 4),
                new SeedCard("Frost Auger", KaldheimSet.SetCode, nameof(CardSearchGroup.Blue), 4),
                
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
        }

        private SeedSet ColdsnapSet { get; set; }
        private SeedSet ModernHorizonsSet { get; set; }
        private SeedSet KaldheimSet { get; set;  }
        
        private List<SeedCard> SeedCards { get; }
        
        public List<SeedSet> SeedSets => new List<SeedSet>()
        {
            KaldheimSet, ModernHorizonsSet, ColdsnapSet
        };
        
        public List<string> SeedSetCodes { get; }
    }

    public class SeedSet
    {
        public SeedSet(string setCode)
        {
            SetCode = setCode;
        }
        
        public string SetCode { get; set; }
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