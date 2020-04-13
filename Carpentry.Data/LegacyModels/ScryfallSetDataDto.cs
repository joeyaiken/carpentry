using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.LegacyModels
{
    public class ScryfallSetDataDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string ReleaseDate { get; set; }

        public List<ScryfallMagicCard> Cards { get; set; }
    }
}
