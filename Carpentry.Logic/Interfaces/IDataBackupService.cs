using Carpentry.Logic.Models;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IDataBackupService
    {
        Task BackupDatabase();

        Task BackupDatabase(string directory);

        Task<BackupDetailDto> GetBackupDetail(string directory);
    }
}
