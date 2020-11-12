using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Logic.Models
{
    public class AppFiltersDto
    {
        public List<FilterOption> Sets { get; set; }
        public List<FilterOption> Types { get; set; }
        public List<FilterOption> Formats { get; set; }
        public List<FilterOption> Colors { get; set; }
        public List<FilterOption> Rarities { get; set; }
        public List<FilterOption> Statuses { get; set; }
        public List<FilterOption> GroupBy { get; set; }
        public List<FilterOption> SortBy { get; set; }
        public List<FilterOption> SearchGroups { get; set; }
    }
}
