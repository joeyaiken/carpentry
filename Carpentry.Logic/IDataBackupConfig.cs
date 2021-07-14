namespace Carpentry.Logic
{
    /// <summary>
    /// Contains config values required for the DataExport and DataImport services
    /// Specifically, it countains the default file names for the contents of a Carpentry inventory backup
    /// </summary>
    public interface IDataBackupConfig
    {
        string DeckBackupFilename { get; set; }
        string CardBackupFilename { get; set; }
        string PropsBackupFilename { get; set; }
    }
}
