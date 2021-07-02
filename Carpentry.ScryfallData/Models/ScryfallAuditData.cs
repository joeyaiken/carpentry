using System;
using System.ComponentModel.DataAnnotations;

namespace Carpentry.ScryfallData.Models
{
    public class ScryfallAuditData
    {
        [Key]
        public int SetDataAuditKey { get; set; }
        public DateTime? DefinitionsLastUpdated { get; set; }
    }
}
