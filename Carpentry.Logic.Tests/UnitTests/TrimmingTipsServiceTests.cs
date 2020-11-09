using Carpentry.Logic.Implementations;
using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests.UnitTests
{
    [TestClass]
    public class TrimmingTipsServiceTests
    {
        [TestMethod]
        public async Task GetTrimmingTips_ThrowsNotImplemented_Test()
        {
            Assert.Fail("Not Implemented");
            //var trimmingTipsService = new TrimmingTipsService();
            //await Assert.ThrowsExceptionAsync<NotImplementedException>(() => trimmingTipsService.GetTrimmingTips());
        }

        [TestMethod]
        public async Task HideTrimmingTip_ThrowsNotImplemented_Test()
        {
            Assert.Fail("Not Implemented");
            //var trimmingTipsService = new TrimmingTipsService();
            //var payloadToSubmit = new InventoryOverviewDto();
            //await Assert.ThrowsExceptionAsync<NotImplementedException>(() => trimmingTipsService.HideTrimmingTip(payloadToSubmit));
        }
    }
}
