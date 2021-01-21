using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class ValidatedDeckImportDto
    {
        public ValidatedDeckImportDto()
        {
            DeckProps = new DeckPropertiesDto();
            UntrackedSets = new List<ValidatedDtoUntrackedSet>();
            ValidatedCards = new List<ValidatedCardDto>();
            //InvalidCards = new List<ImportListRecord>();
            //InvalidRows = new List<string>();
        }

        public bool IsValid { get; set; }

        public DeckPropertiesDto DeckProps { get; set; }
        public List<ValidatedDtoUntrackedSet> UntrackedSets { get; set; }
        public List<ValidatedCardDto> ValidatedCards { get; set; }

        //These two should be refactored into the ValidatedCardDto
        //  including IsValid 
        //public List<ImportListRecord> InvalidCards { get; set; }
        //public List<string> InvalidRows { get; set; }
    }

    //Represents a validated row of the import text
    public class ValidatedCardDto
    {
        public ValidatedCardDto()
        {
            IsValid = true;
            IsEmpty = true;
            Tags = new List<string>();
        }
        public string SourceString { get; set; } //raw unparsed record
        public bool IsValid { get; set; }
        public bool IsBasicLand { get; set; }
        public bool IsEmpty { get; set; }

        //parsed card data
        public int Count { get; set; }
        public string Name { get; set; }
        public char? Category { get; set; }

        //Should these be null when empty card?
        //Should they be a class instead of flat props?
        public int CardId { get; set; }
        public string SetCode { get; set; }
        public int CollectorNumber { get; set; }
        public bool IsFoil { get; set; }
        public List<string> Tags { get; set; }
    }

    //public class ImportListRecord
    //{
    //    public int Count { get; set; }
    //    public string Name { get; set; }
    //    public char? Category { get; set; }

    //    //optional (not set when card empty)
    //    public string Code { get; set; }
    //    public int Number { get; set; }
    //    public bool IsFoil { get; set; }
    //}
}
