using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Interfaces
{
    public interface IDataBackupService
    {

        Task BackupDatabase();

    }
}
