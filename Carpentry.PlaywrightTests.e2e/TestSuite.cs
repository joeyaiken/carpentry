﻿using Microsoft.Playwright;
using System.Threading.Tasks;
using Carpentry.PlaywrightTests.Common;
using Carpentry.PlaywrightTests.Common.Tests;
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
        private readonly AppSettings _appSettings;
        private static int APP_INIT_DELAY = 5000;
        private readonly SeedData _seedData;
        private readonly ILogger _logger;
        
        public TestSuite(
            AppSettings appSettings,
            ILogger logger
            )
        {
            _appSettings = appSettings;
            _seedData = new SeedData();
            _logger = logger;
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

            var page = await browser.NewPageAsync();

            await WaitForApp(page);

            //TODO - Find an appropriate way to load these with DI instead of constructing them manually (or something more graceful)


            // var test0 = new Test00HomePageLoads(page, _appSettings.AppUrl, _logger);
            // await test0.Run();

            await new Test00HomePageLoads(page, _appSettings.AppEnvironment, _appSettings.AppUrl, _logger).Run();
            
            // await new Test01SettingsTrackSnowSets(page, _appSettings.AppUrl, _logger).Run();
            
            // await new Test02InventoryAddSnowCards(page, _appSettings.AppUrl, _seedData, _logger, _appSettings.AppEnvironment).Run();
            
            await new Test03ConfirmInventoryCards(page, _appSettings.AppUrl, _seedData, _appSettings.AppEnvironment).Run();

            
            _logger.Information("Finished running test suite");
        }

        //Current TODO - just get the first test suite working before refining this
        
        private async Task WaitForApp(IPage page)
        {
            _logger.Information($"Beginning {nameof(WaitForApp)}");

            bool pageIsLoaded = false;
            while (!pageIsLoaded)
            {
                //TODO - implement some limit/timeout
                //Either this properly awaits the page, or fails and should delay
                try //TODO - catch specific errors, don't catch blindly
                {
                    _logger.Information("TryLoadPage");
                    await page.GotoAsync(_appSettings.AppUrl, new PageGotoOptions() { Timeout = 0 }); //TODO add a natural timeout instead of handling an exception
                    _logger.Information("Page Loaded");
                    pageIsLoaded = true;
                }
                catch
                {
                    _logger.Information($"Error loading page, delaying before retrying...");
                    await Task.Delay(APP_INIT_DELAY);
                }
            }

            bool appIsLoaded = false;
            while (!appIsLoaded)
            {
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
    }
}