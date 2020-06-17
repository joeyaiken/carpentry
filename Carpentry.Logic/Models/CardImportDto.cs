using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public enum CardImportPayloadType { Arena, Carpentry }

    public class CardImportDto //Should this be a "rawCardImportDto"?
    {
        public CardImportPayloadType ImportType { get; set; }
        public string ImportPayload { get; set; }
        //Thoughts: props can just be added on a validated payload
    }
}
