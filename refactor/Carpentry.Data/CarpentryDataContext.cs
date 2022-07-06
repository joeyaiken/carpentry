using Carpentry.CarpentryData.Models;
using Carpentry.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Data;

public partial class CarpentryDataContext : DbContext
{
    #region DbSets
    
    public DbSet<CoreDefinitionUpdateHistoryData> CoreDefinitionUpdateHistory { get; set; }
    public DbSet<CardSetData> Sets { get; set; }
    

    #endregion
    
    public CarpentryDataContext(DbContextOptions<CarpentryDataContext> options
        // , ILogger<CarpentryDataContext> logger
        ) : base(options)
    {
        // _logger = logger;
        //DB should be set on the configuration page
    }
    
}