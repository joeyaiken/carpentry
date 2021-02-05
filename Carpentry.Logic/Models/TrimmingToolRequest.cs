using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class TrimmingToolRequest
    {
        public string SetCode { get; set; }

        public string SearchGroup { get; set; }

        public int MinCount { get; set; }

        //public string MinBy { get; set; }
        public string FilterBy { get; set; }
    }
}
