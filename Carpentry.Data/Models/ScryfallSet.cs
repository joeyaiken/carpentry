using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.Models
{
    //ScryfallSet
    public class ScryfallSet
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime ReleasedAt { get; set; }

        public DateTime? LastUpdated { get; set; }

        public string CardData { get; set; }

        public bool DataIsParsed { get; set; }

    }
}
