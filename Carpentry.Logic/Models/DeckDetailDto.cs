using System.Collections.Generic;

namespace Carpentry.Logic.Models
{
    public class DeckDetailDto
    {
        public DeckPropertiesDto Props { get; set; }
        public List<DeckCardOverview> Cards { get; set; }
        public DeckStatsDto Stats { get; set; }
    }
}
