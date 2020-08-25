﻿using System;
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
        }

        //Should this also have a "existingDeckId" or do I just exclusively set the ID of DeckProps?
        public DeckPropertiesDto DeckProps { get; set; }
        public List<ValidatedCardDto> ValidatedCards { get; set; }
        public List<ValidatedDtoUntrackedSet> UntrackedSets { get; set; }
        //public List<string> UntrackedSets { get; set; }

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

    public class ValidatedCardVariant
    {
        public int VariantTypeId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

}
