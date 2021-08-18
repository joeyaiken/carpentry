using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Carpentry.CarpentryData.Tests
{
    [TestClass]
    public class SeedDataTests
    {
        private CarpentryDataContext _cardContext;
        private DbContextOptions<CarpentryDataContext> _cardContextOptions;

        private void ResetContext()
        {
            var mockDbLogger = new Mock<ILogger<CarpentryDataContext>>();
            _cardContext = new CarpentryDataContext(_cardContextOptions, mockDbLogger.Object);
        }

        [TestInitialize]
        public void BeforeEach()
        {
            _cardContextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
                .UseSqlite("Filename=CarpentryData.db").Options;
            ResetContext();
        }

        [TestCleanup]
        public async Task AfterEach()
        {
            await _cardContext.Database.EnsureDeletedAsync();
            await _cardContext.DisposeAsync();
        }

        [DataRow(true)]
        [DataRow(false)]
        [DataTestMethod]
        public async Task SeedData_Works_Test(bool includeViews)
        {
            await _cardContext.EnsureDatabaseCreated(includeViews);

            ResetContext();

            //Assert.Fail("Must implement more details about expected record count");
        }


        //TODO - Add tests that cover view logic (or add them in a 2nd test class)
    }
}
