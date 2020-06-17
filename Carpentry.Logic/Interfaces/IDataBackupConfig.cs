namespace Carpentry.Logic.Interfaces
{
    public interface IDataBackupConfig
    {
        //string DatabaseLocation { get; set; }

        //string DeckBackupLocation { get; set; }

        //string CardBackupLocation { get; set; }

        //string PropsBackupLocation { get; set; }

        string BackupDirectory { get; set; }

        string DeckBackupFilename { get; set; }
        string CardBackupFilename { get; set; }
        string PropsBackupFilename { get; set; }
    }
}
