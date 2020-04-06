using Carpentry.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Tools.QuickBackup
{
    class BackupToolConfig : IDataBackupConfig
    {
        public BackupToolConfig(IConfiguration appConfig)
        {
            //Database
            string backupSourceDbRoot = appConfig.GetValue<string>("AppSettings:DatabaseFolderPath");
            string backupSourceDbLocation = appConfig.GetValue<string>("AppSettings:DatabaseFilename");
            DatabaseLocation = $"{backupSourceDbRoot}{backupSourceDbLocation}";

            //Backups
            string backupFolderPath = appConfig.GetValue<string>("AppSettings:BackupFolderPath");
            string deckBackupFilename = appConfig.GetValue<string>("AppSettings:DeckBackupFilename");
            string cardsBackupFilename = appConfig.GetValue<string>("AppSettings:CardBackupFilename");
            string propsBackupFilename = appConfig.GetValue<string>("AppSettings:PropsBackupFilename");
            DeckBackupLocation = $"{backupFolderPath}{deckBackupFilename}";
            CardBackupLocation = $"{backupFolderPath}{cardsBackupFilename}";
            PropsBackupLocation = $"{backupFolderPath}{propsBackupFilename}";
        }

        public string DatabaseLocation { get; set; }

        public string DeckBackupLocation { get; set; }

        public string CardBackupLocation { get; set; }

        public string PropsBackupLocation { get; set; }        
    }
}
