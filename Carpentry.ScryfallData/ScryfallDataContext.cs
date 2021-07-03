using Carpentry.ScryfallData.Models;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.ScryfallData
{
    /// <summary>
    /// The Scryfall repo is just a simple repository of entire Scryfall sets, and an unconnected audit table
    /// The repo exists so scryfall definitions can be stored locally, preventing the API from being called too frequently
    /// </summary>
    public class ScryfallDataContext : DbContext
    {
        public DbSet<ScryfallAuditData> ScryfallAuditData { get; set; }
        public DbSet<ScryfallSetData> Sets { get; set; }

        //DB should be set on the configuration page
        public ScryfallDataContext(DbContextOptions<ScryfallDataContext> options) : base(options) { }

        //No associations to set up for this DB
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
