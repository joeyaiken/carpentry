using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class ScryfallAuditData
    {
        [Key]
        public int SetDataAuditKey { get; set; }

        public DateTime? DefinitionsLastUpdated { get; set; }

    }
}
