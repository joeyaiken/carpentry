using Carpentry.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Data.DataContext
{
    public class ScryfallDataContext : DbContext
    {
        public DbSet<ScryfallAuditData> AuditData { get; set; }
     
        public DbSet<ScryfallSetData> Sets { get; set; }

        public ScryfallDataContext(DbContextOptions<ScryfallDataContext> options) : base(options)
        {
            //DB should be set on the configuration page
        }

        //No associations to set up for this DB
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }   
}
