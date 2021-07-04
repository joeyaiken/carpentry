using System;
using System.ComponentModel.DataAnnotations;

namespace Carpentry.ScryfallData.Models
{
    public class ScryfallAuditData //TODO - Refactor into something that stores the API result of all available sets
    {
        [Key]
        public int SetDataAuditKey { get; set; }
        public DateTime? DefinitionsLastUpdated { get; set; }
    }
}
