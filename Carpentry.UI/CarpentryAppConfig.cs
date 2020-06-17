using Carpentry.Logic.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Carpentry.UI
{
    // - Find a way to refactor this out?
    //WAIT - I DON'T want to just refactor it out, I want a CarpentryAppConfig
    //This would hold backup info, update frequency info (I guess), and any other config props
    //(what props would I actually want?)

    //The idea is to have a single class that can verify if the expected app settings are present (and I guess default if things are empty)
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
