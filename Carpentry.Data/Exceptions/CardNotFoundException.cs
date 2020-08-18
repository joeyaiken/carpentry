using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.Exceptions
{
    public class CardNotFoundException : Exception
    {
        public CardNotFoundException()
        {

        }

        public CardNotFoundException(string setCode, string name)
            : base($"Could not find card ({setCode}) {name}")
        {

        }
        
        public CardNotFoundException(string setCode, int collectorNumber)
            : base($"Could not find card {setCode}-{collectorNumber}")
        {

        }
    }
}
