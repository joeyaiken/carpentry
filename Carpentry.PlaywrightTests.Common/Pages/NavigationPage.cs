using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Carpentry.PlaywrightTests.Common.Pages
{
    public class NavigationPage : PageBase
    {
        private readonly string _pageUrl;
        private readonly string _heading;

        protected NavigationPage(IPage page, AppType appType, string pageUrl, string heading) : base(page, appType)
        {
            _pageUrl = pageUrl;
            _heading = heading;
        }
        
        public async Task NavigateTo()
        {
            if (_pageUrl == Page.Url) return;
            await Page.ClickAsync($"#app-nav-menu .nav-button:has-text(\"{_heading}\")");
        }
    }
}