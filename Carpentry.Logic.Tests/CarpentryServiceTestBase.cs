using Carpentry.CarpentryData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    public abstract class CarpentryServiceTestBase
    {

        protected CarpentryDataContext CardContext = null!;

        private DbContextOptions<CarpentryDataContext> _cardContextOptions = null!;

        [TestInitialize]
        public async Task BeforeEach()
        {
            _cardContextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
                .UseSqlite("Filename=CarpentryData.db").Options;
            
            ResetContext();
            
            await CardContext.EnsureDatabaseCreated(false);
            
            ResetContext();

            await BeforeEachChild();
        }

        protected abstract Task BeforeEachChild();

        [TestCleanup]
        public async Task AfterEach()
        {
            await CardContext.Database.EnsureDeletedAsync();
            await CardContext.DisposeAsync();

            await AfterEachChild();
        }

        protected abstract Task AfterEachChild();

        protected void ResetContext()
        {
            var mockDbLogger = new Mock<ILogger<CarpentryDataContext>>();
            CardContext = new CarpentryDataContext(_cardContextOptions, mockDbLogger.Object);

            ResetContextChild();
        }

        protected abstract void ResetContextChild();
    }
}
