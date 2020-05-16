using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class BackupDetailDto
    {
        public string Directory { get; set; }

        public DateTime TimeStamp { get; set; }

        public List<string> SetCodes { get; set; }
    }
}
