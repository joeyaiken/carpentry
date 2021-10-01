using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e;
using Carpentry.PlaywrightTests.e2e.Tests;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpentry.PlaywrightTests
{
    [TestClass]
    public class SeedTests
    {


        // //Assumes an app is actively running (dumb i know)
        // [TestMethod]
        // public async Task Test02InventoryAddSnowCards_Works()
        // {
        //     using var playwright = await Playwright.CreateAsync();
        //     await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        //     {
        //         Headless = false,
        //         //SlowMo = 50,
        //     });
        //     var page = await browser.NewPageAsync();
        //
        //     var seedData = new SeedData();
        //     var options = Options.Create(new AppSettings() { AppUrl = "https://localhost:44335/"});
        //     //Create a Test02InventoryAddSnowCards
        //     var runnable = new Test02InventoryAddSnowCards(page, options, seedData);
        //
        //     await runnable.Run();
        // }
    }
}