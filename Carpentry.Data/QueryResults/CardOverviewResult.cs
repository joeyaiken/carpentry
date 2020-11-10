namespace Carpentry.Data.QueryResults
{
    public class CardOverviewResult
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public string SetCode { get; set; }
        public char RarityId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Cost { get; set; }
        public int? Cmc { get; set; }
        public string Text { get; set; }
        public string Img { get; set; }
        public int Count { get; set; }

        //category / status / group
        //public string Description { get; set; }


        public string Category { get; set; }
        public decimal? Price { get; set; }
        public bool? IsFoil { get; set; }
        public int? CollectorNumber { get; set; }
        public string Color { get; set; }
        public string ColorIdentity { get; set; }

    }
}
