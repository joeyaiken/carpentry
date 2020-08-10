using Carpentry.Logic.Models;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    //I really don't like "Data Backup Service"
    //It's an Inventory Export Service
    public interface IDataBackupService
    {
        Task BackupCollectionToDirectory(string directory);
        Task<byte[]> GenerateZipBackup();
    }
}
