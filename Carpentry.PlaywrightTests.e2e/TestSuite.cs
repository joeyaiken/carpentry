﻿using Carpentry.PlaywrightTests.e2e.Pages;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public TestSuite(
            IOptions<AppSettings> appSettings
            )
        {
            _appSettings = appSettings.Value;
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




            //await DemoTest();
            //return;

            var page = await browser.NewPageAsync();

            Console.WriteLine("Going to page");

            //await page.GotoAsync(APP_URL);

            await WaitForApp(page);

            Console.WriteLine($"starting {nameof(TestHomePageLoads)}");

            await TestHomePageLoads(page);

            //navigate to settings
            //add desired tracked sets (the 3 needed for SimicSnowStompy)

            await TestSettingsLoads(page);

            //navigate to Inventory

            //click Add Cards

            //add cards by set
            //MH1
            //Kaldheim
            //Coldsnap

        }



        private async Task TestHomePageLoads(IPage page)
        {
            var homePage = new HomePage(_appSettings.AppUrl, page);

            await homePage.NavigateTo();

            Assert.AreEqual("Carpentry", homePage.TitleText);

            Assert.AreEqual("A deck & inventory management tool for Magic the Gathering", homePage.SubtitleText);

            //TODO - Make assertions about the deck list
            Console.WriteLine($"{nameof(TestHomePageLoads)} completed successfully");
        }

        private async Task TestSettingsLoads(IPage page)
        {
            var settingsPage = new SettingsPage(_appSettings.AppUrl, page);
            await settingsPage.NavigateTo();

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
            //bool pageIsLoaded = await TryLoadPage(page);
            bool pageIsLoaded = false;
            while (!pageIsLoaded) pageIsLoaded = await TryLoadPage(page);

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

            while (!appIsLoaded) appIsLoaded = await TryLoadApp(page);
            Console.WriteLine($"{nameof(WaitForApp)} completed");
        }

        private async Task<bool> TryLoadPage(IPage page)
        {
            //TODO - implement some limit/timeout
            //Either this properly awaits the page, or fails and should delay
            try //TODO - catch specific errors, don't catch blindly
            {
                Console.WriteLine("TryLoadPage");
                await page.GotoAsync(_appSettings.AppUrl, new PageGotoOptions() { Timeout = 0 }); //TODO add a natural timeout instead of handling an exception
                Console.WriteLine("Page Loaded");
                return true;
            }
            catch //(PlaywrightException ex)
            {
                //Console.WriteLine($"Page Not loaded: {ex.Message}");
                Console.WriteLine($"Error loading page, delaying before retrying...");
                await Task.Delay(APP_INIT_DELAY);
                return false;
            }
        }

        private async Task<bool> TryLoadApp(IPage page)
        {
            try //TODO - catch specific errors, don't catch blindly
            {
                Console.WriteLine("Checking app status (TryLoadApp)");
                //await page.GotoAsync(APP_URL);
                var appText = await page.TextContentAsync("app-root");
                if (appText != "Loading...")
                {
                    Console.WriteLine("App loaded!");
                    return true;
                };
            }
            catch
            {
                Console.WriteLine("Error loading app");
            }

            Console.WriteLine("App not loaded, delaying before retrying...");
            await Task.Delay(APP_INIT_DELAY);
            return false;

        }

    }

}
