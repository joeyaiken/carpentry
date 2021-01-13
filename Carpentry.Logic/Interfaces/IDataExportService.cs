using Carpentry.Logic.Models;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    //I really don't like "Data Backup Service"
    //It's an Inventory Export Service
    public interface IDataExportService
    {
        Task BackupCollectionToDirectory(string directory);
        Task<byte[]> GenerateZipBackup();

        //Task<string> GetDeckListExport(int deckId);
    }
}
