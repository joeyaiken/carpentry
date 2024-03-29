﻿using Carpentry.PlaywrightTests.Common;

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
