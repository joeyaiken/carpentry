using System;

namespace Carpentry.DataLogic.Exceptions
{
    public class CardNotFoundException : Exception
    {
        public CardNotFoundException() { }

        public CardNotFoundException(string message, Exception innerException) { }

        public CardNotFoundException(string message) :base(message) { }

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
