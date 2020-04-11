using Carpentry.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Data.DataContext
{
    public class ScryfallDataContext : DbContext
    {
        //public DbSet<Card> Cards { get; set; }
        public DbSet<ScryfallSet> Sets { get; set; }

        public ScryfallDataContext(DbContextOptions<ScryfallDataContext> options) : base(options)
        {
            //DB should be set on the configuration page
        }

        //No associations to set up for this DB
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //}

    }   
}
