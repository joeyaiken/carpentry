using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.PlaywrightTests.e2e
{
    public class AppSettings
    {
        public string AngularUrl { get; set; }
        public string ReactUrl { get; set; }
        
        public string AppUrl { get; set; }
        public AppType AppEnvironment { get; set; }
    }
}
