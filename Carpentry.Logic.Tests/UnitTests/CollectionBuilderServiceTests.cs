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
    public class CollectionBuilderServiceTests
    {
        [TestMethod]
        public async Task GetTrimmingTips_ThrowsNotImplemented_Test()
        {
            var collectionService = new CollectionBuilderService();
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => collectionService.GetCollectionBuilderSuggestions());
        }

        [TestMethod]
        public async Task HideTrimmingTip_ThrowsNotImplemented_Test()
        {
            var collectionService = new CollectionBuilderService();
            var payloadToSubmit = new InventoryOverviewDto();
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => collectionService.HideCollectionBuilderSuggestion(payloadToSubmit));
        }
    }
}
