using Carpentry.Data.Models;
using Carpentry.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.UI.Util
{
    public class Mapper
    {

        Mapper()
        {

        }






        public static DeckProperties DeckProperties_DtoToData(DeckPropertiesDto dto)
        {
            DeckProperties result = new DeckProperties()
            {
                BasicB = dto.BasicB,
                BasicG = dto.BasicG,
                BasicR = dto.BasicR,
                BasicU = dto.BasicU,
                BasicW = dto.BasicW,
                Format = dto.Format,
                Id = dto.Id,
                Name = dto.Name,
                Notes = dto.Notes,
            };
            return result;
        }

        public static DeckPropertiesDto DeckProperties_DataToDto(DeckProperties dto)
        {
            DeckPropertiesDto result = new DeckPropertiesDto()
            {
                BasicB = dto.BasicB,
                BasicG = dto.BasicG,
                BasicR = dto.BasicR,
                BasicU = dto.BasicU,
                BasicW = dto.BasicW,
                Format = dto.Format,
                Id = dto.Id,
                Name = dto.Name,
                Notes = dto.Notes,
            };
            return result;
        }



    }
}
