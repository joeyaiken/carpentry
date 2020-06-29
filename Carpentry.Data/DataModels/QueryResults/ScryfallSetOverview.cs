using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels.QueryResults
{
    public class ScryfallSetOverview
    {
        //code
        public string Code { get; set; }
        //name
        public string Name { get; set; }
        // ? is tracked?
        public DateTime? LastUpdated { get; set; }
    }
}
