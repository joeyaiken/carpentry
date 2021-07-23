using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{

    /// <summary>
    /// Tests the ability of the DataUpdateService and ScryfallService 
    /// </summary>HeyAw
    [TestClass]
    public class ScryfallUpdateIntegrationTests : CarpentryServiceTestBase
    {
        private DataUpdateService _updateService = null!;
        private ScryfallService _scryService = null!;

        protected override bool SeedViews => true;

        protected override Task BeforeEachChild()
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        protected override Task AfterEachChild()
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        protected override void ResetContextChild()
        {
            var handlerMock = new MockHttpClient();
            var mockScryLogger = new Mock<ILogger<ScryfallService>>();
            _scryService = new ScryfallService(ScryContext, mockScryLogger.Object, handlerMock.HttpClient);

            var mockUpdateLogger = new Mock<ILogger<DataUpdateService>>();
            var mockIntegrityService = new Mock<IDataIntegrityService>(MockBehavior.Strict);
            _updateService = new DataUpdateService(mockUpdateLogger.Object, _scryService, mockIntegrityService.Object, CardContext);
        }

        /* So really this is trying to test a workflow more than an individual unit of code
         * I can't add/inspect a specific set without first adding a set or two
         * So, should I just have a single integration/e2e test that processes the real data?
         *  (I can delete the tracked sets afterwards)

            Get tracked sets, update, include untracked

            Add [some problematic sets]
         
            Workflow_Handles[]Sets_Test()
            UpdateService_HandlesRealSets_Workflow_Test()

         */
        [TestMethod]
        public async Task UpdateService_HandlesRealSets_Workflow_Test()
        {
            await _updateService.TryUpdateAvailableSets();

            var allSets = await _updateService.GetTrackedSets(false, true);

            var setCodesById = allSets.ToDictionary(s => s.Code.ToLower(), s => s.SetId);

            var setCodesToAdd = new List<string>() { "war", "eld", "thb", "mh2" };

            foreach(var setCode in setCodesToAdd)
                await _updateService.AddTrackedSet(setCodesById[setCode]);

            //query for sets
            var trackedSets = await CardContext.Sets
                .Where(s => s.IsTracked == true)
                //.Include(s => s.Cards)
                .ToListAsync();

            Assert.AreEqual(setCodesToAdd.Count, trackedSets.Count);
        }


        



    }
}
