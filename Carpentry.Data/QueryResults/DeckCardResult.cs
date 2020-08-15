namespace Carpentry.Data.QueryResults
{
    public class DeckCardResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Cost { get; set; }

        public int? Cmc { get; set; }

        public string Img { get; set; }

        public string Category { get; set; }

        public string Set { get; set; }

        public bool IsFoil { get; set; }

        //public string VariantType { get; set; }
        public int? CollectorNumber { get; set; }
        public int CardId { get; set; }
    }
}
