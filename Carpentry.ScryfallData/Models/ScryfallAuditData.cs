using System;
using System.ComponentModel.DataAnnotations;

namespace Carpentry.ScryfallData.Models
{
    //Database object that represents the query for all scryfall sets
    //  (can safely be cached since this realistically only be updated daily at most)
    //TODO - Rename to something else without the word 'audit', it's a stored collection of set tokens in a single record
    public class ScryfallAuditData
    {
        [Key]
        public int SetDataAuditKey { get; set; }
        public string SetTokensString { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
