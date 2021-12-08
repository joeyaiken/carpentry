using Carpentry.Logic;

namespace Carpentry.Tools.QuickImport
{
    class FakeAppConfig : IDataBackupConfig
    {
        public string DeckBackupFilename { get; set; }
        public string CardBackupFilename { get; set; }
        public string PropsBackupFilename { get; set; }
    }
}
