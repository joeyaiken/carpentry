using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Carpentry.PlaywrightTests.e2e.Pages
{
    public class InventoryAddCardsPage
    {
        private readonly string _pageUrl;
        private readonly IPage _page;
        public InventoryAddCardsPage(string appUrl, IPage page)
        {
            _pageUrl = $"{appUrl}inventory/add-cards";
            _page = page;
        }

        public async Task NavigateTo()
        {
            if (_pageUrl == _page.Url) return;
            // await _page.GotoAsync(_pageUrl);
            await _page.ClickAsync("app-nav-menu a:has-text(\"Inventory\")");
            await _page.ClickAsync("app-inventory-overview button:has-text(\"Add Cards\")");
            //
        }
        
    }
}