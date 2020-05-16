using Carpentry.Logic.Interfaces;
using Microsoft.Extensions.Configuration;

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
            BackupDirectory = appConfig.GetValue<string>("AppSettings:BackupFolderPath");

            DeckBackupFilename = appConfig.GetValue<string>("AppSettings:DeckBackupFilename");
            CardBackupFilename = appConfig.GetValue<string>("AppSettings:CardBackupFilename");
            PropsBackupFilename = appConfig.GetValue<string>("AppSettings:PropsBackupFilename");
            //DeckBackupLocation = $"{backupFolderPath}{deckBackupFilename}";
            //CardBackupLocation = $"{backupFolderPath}{cardsBackupFilename}";
            //PropsBackupLocation = $"{backupFolderPath}{propsBackupFilename}";
        }
        //DB
        public string DatabaseLocation { get; set; }


        //Backups
        public string BackupDirectory { get; set; }

        public string DeckBackupFilename { get; set; }
        public string CardBackupFilename { get; set; }
        public string PropsBackupFilename { get; set; }






        //public string DeckBackupLocation { get; set; }

        //public string CardBackupLocation { get; set; }

        //public string PropsBackupLocation { get; set; }        
    }
}
