using Carpentry.CarpentryData;
using Carpentry.ScryfallData;
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
    public class ScryfallUpdateIntegrationTests// : CarpentryServiceTestBase
    {
        private DataUpdateService _updateService = null!;
        private ScryfallService _scryService = null!;

        private CarpentryDataContext CardContext = null!;
        private ScryfallDataContext ScryContext = null!;

        private DbContextOptions<CarpentryDataContext> _cardContextOptions = null!;
        private DbContextOptions<ScryfallDataContext> _scryContextOptions = null!;


        [TestInitialize]
        public async Task BeforeEach()
        {
            _cardContextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
                .UseSqlite("Filename=CarpentryData.db").Options;
            _scryContextOptions = new DbContextOptionsBuilder<ScryfallDataContext>()
                .UseSqlite("Filename=ScryData.db").Options;

            ResetContext();

            await CardContext.EnsureDatabaseCreated(false);
            await ScryContext.Database.EnsureCreatedAsync();

            ResetContext();
        }

        [TestCleanup]
        public async Task AfterEach()
        {
            await CardContext.Database.EnsureDeletedAsync();
            //await CardContext.DisposeAsync();

            //await ScryContext.Database.EnsureDeletedAsync();
            await ScryContext.DisposeAsync();
        }

        private void ResetContext()
        {
            var mockDbLogger = new Mock<ILogger<CarpentryDataContext>>();
            CardContext = new CarpentryDataContext(_cardContextOptions, mockDbLogger.Object);
            ScryContext = new ScryfallDataContext(_scryContextOptions);

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
            //If nothing is tracked, add sets to track
            var actualSetCount = await CardContext.Sets.CountAsync();
            if(actualSetCount == 0) await _updateService.TryUpdateAvailableSets();

            var allSets = await _updateService.GetTrackedSets(true, false);

            var setCodesById = allSets.ToDictionary(s => s.Code.ToLower(), s => s);

            var setCodesToAdd = new List<string>() { "war", "eld", "thb", "mh2" };

            foreach(var setCode in setCodesToAdd)
                //if(!(setCodesById.TryGetValue(setCode, out var match) && match.IsTracked))
                if(!setCodesById[setCode].IsTracked)
                    await _updateService.AddTrackedSet(setCodesById[setCode].SetId);
            
            //query for sets
            var trackedSets = await CardContext.Sets
                .Where(s => s.IsTracked == true)
                //.Include(s => s.Cards)
                .ToListAsync();

            var counts = await CardContext.Sets
                .Where(s => s.IsTracked == true)
                .Select(s => new
                {
                    SetCode = s.Code,
                    CardCount = s.Cards.Count()
                })
                .ToDictionaryAsync(s => s.SetCode, s => s.CardCount);

            Assert.AreEqual(setCodesToAdd.Count, trackedSets.Count);
        }


        



    }
}
