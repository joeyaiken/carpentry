﻿namespace Carpentry.Logic.Models
{
    public class TrimmedCardDto
    {
        public string CardName { get; set; }
        public int CardId { get; set; }
        public bool IsFoil { get; set; }
        public int NumberToTrim { get; set; }
    }
}
