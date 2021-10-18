using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Carpentry.PlaywrightTests.e2e.Pages
{
    public class NavigationPage
    {
        private readonly IPage _page;
        private readonly string _pageUrl;
        private readonly string _heading;

        public NavigationPage(IPage page, string pageUrl, string heading)
        {
            _page = page;
            _pageUrl = pageUrl;
            _heading = heading;
        }
        
        public async Task NavigateTo()
        {
            if (_pageUrl == _page.Url) return;
            await _page.ClickAsync($"#app-nav-menu button:has-text(\"{_heading}\")");
        }
    }
}