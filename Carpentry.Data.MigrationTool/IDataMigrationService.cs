using System.Threading.Tasks;

namespace Carpentry.Data.MigrationTool
{
    public interface IDataMigrationService
    {
               
        Task RestoreDB();
        
        Task RefreshDB();
    }
}
