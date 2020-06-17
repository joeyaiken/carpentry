using Carpentry.Logic.Models;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IDataBackupService
    {
        //Task BackupDatabase();

        //Task BackupDatabase(string directory);

        //Task<BackupDetailDto> GetBackupDetail(string directory);

        //
        //
        //

        //public async BackupDetailDto VerifyBackupLocation(string directory)
        Task<BackupDetailDto> VerifyBackupLocation(string directory);

        //public async void BackupCollection(string directory)
        Task BackupCollection();
        Task BackupCollection(string directory);


        //public async void RestoreCollectionFromBackup(string directory)
        Task RestoreCollectionFromBackup();
        Task RestoreCollectionFromBackup(string directory);


        //Task BackupCollection(string backupDirectory, string deckFilename, string cardFilename, string propsFilename);


        //Task BackupCollection(string deckBackupLocation, string cardBackupLocation, string propsBackupLocation);
        

    }
}
