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
            ValidatedCards = new List<ValidatedCardDto>();
            UntrackedSets = new List<ValidatedDtoUntrackedSet>();
            InvalidCards = new List<ImportListRecord>();
            InvalidRows = new List<string>();
        }

        public bool IsValid { get; set; }
        public DeckPropertiesDto DeckProps { get; set; }
        public List<ValidatedCardDto> ValidatedCards { get; set; }
        public List<ValidatedDtoUntrackedSet> UntrackedSets { get; set; }
        public List<ImportListRecord> InvalidCards { get; set; }
        public List<string> InvalidRows { get; set; }
    }

    public class ValidatedCardDto
    {
        //What data does this actually need?

        //public int MultiverseId { get; set; }
        public int CardId { get; set; }
        public string SetCode { get; set; }
        public string Name { get; set; }
        public int CollectorNumber { get; set; }
        //public List<ValidatedCardVariant> Variants { get; set; }
        //public string VariantName { get; set; }
        public bool IsFoil { get; set; }
    }

    public class ImportListRecord
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Number { get; set; }
        public bool IsFoil { get; set; }

        public char? Category { get; set; }
    }
}
