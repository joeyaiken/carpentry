using System.Threading.Tasks;

namespace Carpentry.Data.MigrationTool
{
    public interface IDataMigrationService
    {
        Task BackupDB();
        
        Task RestoreDB();
        
        Task RefreshDB();
    }
}
