using System;

namespace Carpentry.Data.DataModels.QueryResults
{
    public class ScryfallSetOverview
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime ReleasedAt { get; set; }

        public DateTime? LastUpdated { get; set; }

        public string SetType { get; set; }

        public int CardCount { get; set; }

        public bool Digital { get; set; }

        public bool NonfoilOnly { get; set; }

        public bool FoilOnly { get; set; }
    }
}
