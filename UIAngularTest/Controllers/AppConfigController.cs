using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace UIAngularTest.Controllers
{
    public class AppConfigResult
    {
        public string ConfigString { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AppConfigController : ControllerBase
    {
        private readonly AppConfig _appConfig;

        public AppConfigController(IOptions<AppConfig> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        [HttpGet]
        public async Task<ActionResult<AppConfigResult>> GetAppConfig()
        {
            await Task.Delay(1000);
            
            var result = new AppConfigResult()
            {
                ConfigString = _appConfig.ExampleConfigString,
                LastUpdated = DateTime.Now,
            };

            return result;
        }
    }
}
