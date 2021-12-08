using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Carpentry.PlaywrightTests.Common
{
    public class PageBase
    {
        protected readonly IPage Page;
        private readonly AppType _appType;

        protected PageBase(IPage page, AppType appType)
        {
            Page = page;
            _appType = appType;
        }
        
        protected async Task SetInputValue(string elementId, string value)
        {
            await Page.FillAsync($"#{elementId}", value);
        }
        
        protected async Task SelectOption(string elementId, string value)
        {
            await Page.ClickAsync($"#{elementId}");
            
            if (_appType == AppType.Angular)
            {
                await Task.Delay(100);
                var selector = await Page.WaitForSelectorAsync($"mat-option:has(span:text-is(\"{value}\"))");
                await selector!.ClickAsync();
            }
            else
            {
                var selector = await Page.WaitForSelectorAsync($"li:text-is(\"{value}\")");
                await selector!.ClickAsync();
            }
        }
        
    }
}