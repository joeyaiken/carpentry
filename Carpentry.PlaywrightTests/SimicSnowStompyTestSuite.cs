using System;
using System.Threading.Tasks;
using Carpentry.PlaywrightTests.Common;
using Carpentry.PlaywrightTests.Common.Tests;
using Carpentry.PlaywrightTests.e2e;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;

namespace Carpentry.PlaywrightTests
{
    [TestClass]
    public class SimicSnowAngularTests : SimicSnowTestBase
    {
        public SimicSnowAngularTests() : base(AppType.Angular) { }
    }

    [TestClass]
    public class SimicSnowReactTests : SimicSnowTestBase
    {
        public SimicSnowReactTests() : base(AppType.React) { }
    }

    public class SimicSnowTestBase
    {
        private static int APP_INIT_DELAY = 5000;
        private Mock<ILogger> _mockLogger;
        private IPage _page;
        private readonly AppSettings _appSettings;
        private readonly SeedData _seedData;

        public SimicSnowTestBase(AppType appType)
        {
            _seedData = new SeedData();
            _appSettings = new AppSettings()
            {
                AppEnvironment = appType,
                AngularUrl = "https://localhost:44335/",
                ReactUrl = "http://localhost:23374/",
            };

            _appSettings.AppUrl = _appSettings.AppEnvironment == AppType.Angular
                ? _appSettings.AngularUrl
                : _appSettings.ReactUrl;
        }
        
        //BeforeAll: Attempt to reset the DB
        
        //Assert the app is running in a test mode (somehow, maybe from the core api?)
        
        //BeforeEach: Wait for the page to be loaded

        [TestInitialize]
        public async Task BeforeEach()
        {
            // using var playwright = await Playwright.CreateAsync();
            // await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            // {
            //     Headless = false,
            //     //SlowMo = 50,
            // });
            //
            // _page = await browser.NewPageAsync();
            _mockLogger = new Mock<ILogger>();
            // await WaitForApp(_page);
        }
        
        private async Task WaitForApp(IPage page)
        {
            bool pageIsLoaded = false;
            while (!pageIsLoaded)
            {
                try
                {
                    await page.GotoAsync(_appSettings.AppUrl, new PageGotoOptions() { Timeout = 0 }); //TODO add a natural timeout instead of handling an exception
                    pageIsLoaded = true;
                }
                catch
                {
                    await Task.Delay(APP_INIT_DELAY);
                }
            }

            bool appIsLoaded = false;
            while (!appIsLoaded)
            {
                try
                {
                    var appText = await page.TextContentAsync("#root");
                    if (appText != "Loading...")
                    {
                        appIsLoaded = true;
                        continue;
                    };
                }
                catch
                {
                    // _logger.Information("Error loading app");
                }

                // _logger.Information("App not loaded, delaying before retrying...");
                await Task.Delay(APP_INIT_DELAY);
            }
            // _logger.Information($"{nameof(WaitForApp)} completed");
        }
        
        [TestMethod]
        public async Task Test_00_HomePage_Loads()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false,
            });
            _page = await browser.NewPageAsync();
            await WaitForApp(_page);
            
            var test = new Test00HomePageLoads(_page, _appSettings.AppEnvironment, _appSettings.AppUrl, _mockLogger.Object);
            await test.Run();
        }

        [TestMethod]
        public async Task Test_01_Settings_TrackSnowSets()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false,
            });
            _page = await browser.NewPageAsync();
            await WaitForApp(_page);

            var test = new Test01SettingsTrackSnowSets(_page, _appSettings.AppEnvironment, _appSettings.AppUrl, _mockLogger.Object);
            await test.Run();
        }

        [TestMethod]
        public async Task Test_02_Inventory_AddSnowCards()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false,
            });
            _page = await browser.NewPageAsync();
            await WaitForApp(_page);
            
            var test = new Test02InventoryAddSnowCards(_page, _appSettings.AppUrl, _seedData, _mockLogger.Object, _appSettings.AppEnvironment);
            await test.Run();
        }
        
        [TestMethod]
        public async Task Test_03_Inventory_ConfirmCards()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false,
            });
            _page = await browser.NewPageAsync();
            await WaitForApp(_page);
            
            var test = new Test03ConfirmInventoryCards(_page, _appSettings.AppUrl, _seedData, _appSettings.AppEnvironment);
            await test.Run();
        }
        
        [TestMethod]
        public async Task Test_04_Decks_CreateSnowDeck()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false,
            });
            _page = await browser.NewPageAsync();
            await WaitForApp(_page);
            
            var test = new Test04CreateSnowDeck(_page, _appSettings.AppUrl, _seedData, _appSettings.AppEnvironment);
            await test.Run();
        }
    }
}