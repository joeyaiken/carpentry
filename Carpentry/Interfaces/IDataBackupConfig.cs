using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Interfaces
{
    public interface IDataBackupConfig
    {
        string DatabaseLocation { get; set; }

        string DeckBackupLocation { get; set; }

        string CardBackupLocation { get; set; }

        string PropsBackupLocation { get; set; }
    }
}
