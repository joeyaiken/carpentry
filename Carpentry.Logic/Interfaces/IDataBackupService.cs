using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IDataBackupService
    {
        Task BackupDatabase();
    }
}
