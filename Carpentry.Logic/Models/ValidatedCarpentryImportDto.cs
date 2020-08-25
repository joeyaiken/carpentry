using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class ValidatedCarpentryImportDto
    {
        public string BackupDirectory { get; set; }

        public DateTime BackupDate { get; set; }

        public List<ValidatedDtoUntrackedSet> UntrackedSets { get; set; }

        //validation errors?
    }

    

}
