using Carpentry.Data.Scryfall.Models;
using Carpentry.ScryfallData.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.ScryfallData;

public class ScryfallDataContext : DbContext
{
    public DbSet<AllSetsCachedData> AllSetsCaches { get; set; }
    // public DbSet<CachedSetData> SetDataCaches { get; set; }
    // public DbSet<CardPriceData> CardPrices { get; set; }

    public ScryfallDataContext(DbContextOptions<ScryfallDataContext> options) : base(options) { }
    //protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}