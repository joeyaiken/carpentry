using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class ValidatedCarpentryImportDto
    {
        public string BackupDirectory { get; set; }

        public DateTime BackupDate { get; set; }

        public List<UntrackedSet> UntrackedSets { get; set; }

        //validation errors?
    }

    public class UntrackedSet
    {
        public int SetId { get; set; }
        public string SetCode { get; set; }
    }

}
