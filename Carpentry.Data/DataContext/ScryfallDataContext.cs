using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataContext
{
    public class ScryfallDataContext : DbContext
    {
        //public DbSet<Card> Cards { get; set; }
        public DbSet<ScryfallSet> Sets { get; set; }

        public DbSet<ScryfallCard> Cards { get; set; }

        public ScryfallDataContext(DbContextOptions<ScryfallDataContext> options) : base(options)
        {
            //DB should be set on the configuration page
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //a set has many cards
            modelBuilder.Entity<ScryfallSet>()
                //has key?
                .HasMany(x => x.Cards)
                .WithOne(x => x.Set)
                .HasForeignKey(x => x.ScryfallSetId);

            //many cards belong to a set
            modelBuilder.Entity<ScryfallCard>()
                //has key?
                .HasOne(x => x.Set)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.ScryfallSetId);

        }

    }

    //ScryfallSet
    public class ScryfallSet
    {
        public int Id { get; set; }

        //public string Name { get; set; }

        public string Code { get; set; }

        public DateTime? LastUpdated { get; set; }

        //card associations
        public List<ScryfallCard> Cards { get; set; }
    }

    //ScryfallCard
    public class ScryfallCard
    {
        //TODO - Update this to include name, it will allow "GetMidByName" to no longer call scryfall api
        
        public int Id { get; set; }

        public int MultiverseId { get; set; }
        
        public string StringData { get; set; }

        public int ScryfallSetId { get; set; }

        //Association
        public ScryfallSet Set { get; set; }
    }


}
