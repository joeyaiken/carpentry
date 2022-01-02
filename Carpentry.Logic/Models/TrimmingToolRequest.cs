namespace Carpentry.Logic.Models
{
    public class TrimmingToolRequest
    {
        public string SetCode { get; set; }
        public string SearchGroup { get; set; }
        public int MinCount { get; set; }
        public decimal MaxPrice { get; set; }
        public string FilterBy { get; set; }
    }
}
