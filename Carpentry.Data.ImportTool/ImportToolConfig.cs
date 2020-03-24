using Microsoft.Extensions.Configuration;

namespace Carpentry.Data.ImportTool
{
    public class ImportToolConfig
    {
        public ImportToolConfig(IConfiguration appConfig)
        {
            //string backupRoot = appConfig.GetValue<string>("AppSettings:BackupFolderRoot");

            //string deckBackupLocation = appConfig.GetValue<string>("AppSettings:DeckBackupFilename");
            //string cardsBackupLocation = appConfig.GetValue<string>("AppSettings:CardBackupFilename");
            //string propsBackupLocation = appConfig.GetValue<string>("AppSettings:PropsBackupFilename");

            //DeckBackupLocation = $"{backupRoot}{deckBackupLocation}";
            //CardBackupLocation = $"{backupRoot}{cardsBackupLocation}";
            //PropsBackupLocation = $"{backupRoot}{propsBackupLocation}";

            //string backupSourceDbRoot = appConfig.GetValue<string>("AppSettings:BackupSourceDBFolderRoot");
            //string backupSourceDbLocation = appConfig.GetValue<string>("AppSettings:BackupSourceDBFilename");

            //BackupSourceDbLocation = $"{backupSourceDbRoot}{backupSourceDbLocation}";

            //string restoreDestinationDbRoot = appConfig.GetValue<string>("AppSettings:RestoreDestinationDBFolderRoot");
            //string restoreDestinationCardDbLocation = appConfig.GetValue<string>("AppSettings:RestoreDestinationCardDBFilename");
            //string restoreDestinationScryDbLocation = appConfig.GetValue<string>("AppSettings:RestoreDestinationScryDBFilename");

            //RestoreDestinationCardDbLocation = $"{restoreDestinationDbRoot}{restoreDestinationCardDbLocation}";
            //RestoreDestinationScryDbLocation = $"{restoreDestinationDbRoot}{restoreDestinationScryDbLocation}";
        }

        //public string DeckBackupLocation { get; set; }

        //public string CardBackupLocation { get; set; }

        //public string PropsBackupLocation { get; set; }

        //public string BackupSourceDbLocation { get; set; }

        //public string RestoreDestinationCardDbLocation { get; set; }

        //public string RestoreDestinationScryDbLocation { get; set; }

    }
}
