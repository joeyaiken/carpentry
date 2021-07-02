using Carpentry.Logic.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Carpentry.Core
{
    class CarpentryAppConfig : IDataBackupConfig //, IAnotherInterface
    {
        public CarpentryAppConfig(IConfiguration appConfig)
        {
            //Backups
            BackupDirectory = appConfig.GetValue<string>("AppSettings:Backups:BackupFolderPath");
            DeckBackupFilename = appConfig.GetValue<string>("AppSettings:Backups:DeckBackupFilename");
            CardBackupFilename = appConfig.GetValue<string>("AppSettings:Backups:CardBackupFilename");
            PropsBackupFilename = appConfig.GetValue<string>("AppSettings:Backups:PropsBackupFilename");
        }

        //Backups
        public string BackupDirectory { get; set; }
        public string DeckBackupFilename { get; set; }
        public string CardBackupFilename { get; set; }
        public string PropsBackupFilename { get; set; }
    }
}
