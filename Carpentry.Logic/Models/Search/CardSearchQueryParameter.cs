using System.Collections.Generic;

namespace Carpentry.Logic.Models.Search
{
    public class CardSearchQueryParameter : CardSearchQueryParameterBase
    {
        public bool ExcludeUnowned { get; set; }
    }
}
