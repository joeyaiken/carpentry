using System;
using System.Collections.Generic;
using System.Text;
using Carpentry.Logic.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Carpentry.Tools.QuickRestore
{
    public class BackupConfig : IDataBackupConfig
    {

        public BackupConfig(IConfiguration appConfig)
        {
            //Backups
            string backupFolderRoot = appConfig.GetValue<string>("AppSettings:BackupFolderRoot");
            string deckBackupFilename = appConfig.GetValue<string>("AppSettings:DeckBackupFilename");
            string cardsBackupFilename = appConfig.GetValue<string>("AppSettings:CardBackupFilename");
            string propsBackupFilename = appConfig.GetValue<string>("AppSettings:PropsBackupFilename");
            DeckBackupLocation = $"{backupFolderRoot}{deckBackupFilename}";
            CardBackupLocation = $"{backupFolderRoot}{cardsBackupFilename}";
            PropsBackupLocation = $"{backupFolderRoot}{propsBackupFilename}";

            //Database
            string dbFolderRoot = appConfig.GetValue<string>("AppSettings:DatabaseFolderRoot");
            string cardDbSourceLocation = appConfig.GetValue<string>("AppSettings:CardDbFilename");
            string scryDbScourceLocation = appConfig.GetValue<string>("AppSettings:ScryDbFilename");
            DatabaseLocation = $"{dbFolderRoot}{cardDbSourceLocation}"; //
            ScryDataLocation = $"{dbFolderRoot}{scryDbScourceLocation}";
        }

        public string DatabaseLocation { get; set; }
        public string ScryDataLocation { get; set; }
        public string DeckBackupLocation { get; set; }
        public string CardBackupLocation { get; set; }
        public string PropsBackupLocation { get; set; }
    }
}
