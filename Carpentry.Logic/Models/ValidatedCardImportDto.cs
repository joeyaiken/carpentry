using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class ValidatedCardImportDto
    {
        //Should this also have a "existingDeckId" or do I just exclusively set the ID of DeckProps?
        public DeckPropertiesDto DeckProps { get; set; } 
        public List<ValidatedCardDto> ValidatedCards { get; set; }
    }

    public class ValidatedCardDto
    {
        //data

        //variants (or part of data?)
        
        //selected variant
        
        //isFoil
        
        //count?or 1 per card

    }
}
