using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Carpentry.Data.BackupTool
{
    public class DataBackupConfig
    {
        public DataBackupConfig(IConfiguration appConfig)
        {
            string appRoot = appConfig.GetValue<string>("AppSettings:BackupFolderRoot");

            string deckBackupLocation = appConfig.GetValue<string>("AppSettings:DeckBackupFilename");
            string cardsBackupLocation = appConfig.GetValue<string>("AppSettings:CardBackupFilename");
            string propsBackupLocation = appConfig.GetValue<string>("AppSettings:PropsBackupFilename");

            DeckBackupLocation = $"{appRoot}{deckBackupLocation}";
            CardBackupLocation = $"{appRoot}{cardsBackupLocation}";
            PropsBackupLocation = $"{appRoot}{propsBackupLocation}";
        }

        public string DeckBackupLocation { get; set; }
        
        public string CardBackupLocation { get; set; }

        public string PropsBackupLocation { get; set; }
    }
}
