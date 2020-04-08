namespace Carpentry.Logic.Interfaces
{
    public interface IDataBackupConfig
    {
        string DatabaseLocation { get; set; }

        string DeckBackupLocation { get; set; }

        string CardBackupLocation { get; set; }

        string PropsBackupLocation { get; set; }
    }
}
