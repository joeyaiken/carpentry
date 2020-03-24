using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Carpentry.Data.MigrationTool
{
    public class MigrationToolConfig
    {
        public MigrationToolConfig(IConfiguration appConfig)
        {
            string backupRoot = appConfig.GetValue<string>("AppSettings:BackupFolderRoot");

            string deckBackupLocation = appConfig.GetValue<string>("AppSettings:DeckBackupFilename");
            string cardsBackupLocation = appConfig.GetValue<string>("AppSettings:CardBackupFilename");
            string propsBackupLocation = appConfig.GetValue<string>("AppSettings:PropsBackupFilename");

            DeckBackupLocation = $"{backupRoot}{deckBackupLocation}";
            CardBackupLocation = $"{backupRoot}{cardsBackupLocation}";
            PropsBackupLocation = $"{backupRoot}{propsBackupLocation}";

            string backupSourceDbRoot = appConfig.GetValue<string>("AppSettings:BackupSourceDBFolderRoot");
            string backupSourceDbLocation = appConfig.GetValue<string>("AppSettings:BackupSourceDBFilename");

            BackupSourceDbLocation = $"{backupSourceDbRoot}{backupSourceDbLocation}";

            string restoreDestinationDbRoot = appConfig.GetValue<string>("AppSettings:RestoreDestinationDBFolderRoot");
            string restoreDestinationCardDbLocation = appConfig.GetValue<string>("AppSettings:RestoreDestinationCardDBFilename");
            string restoreDestinationScryDbLocation = appConfig.GetValue<string>("AppSettings:RestoreDestinationScryDBFilename");

            RestoreDestinationCardDbLocation = $"{restoreDestinationDbRoot}{restoreDestinationCardDbLocation}";
            RestoreDestinationScryDbLocation = $"{restoreDestinationDbRoot}{restoreDestinationScryDbLocation}";

            DataRefreshIntervalDays = 10;
        }

        public string DeckBackupLocation { get; set; }

        public string CardBackupLocation { get; set; }

        public string PropsBackupLocation { get; set; }

        public string BackupSourceDbLocation { get; set; }

        public string RestoreDestinationCardDbLocation { get; set; }

        public string RestoreDestinationScryDbLocation { get; set; }

        public int DataRefreshIntervalDays { get; set; }

    }
}
