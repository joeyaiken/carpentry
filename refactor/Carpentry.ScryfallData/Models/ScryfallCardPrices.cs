namespace Carpentry.ScryfallData.Models;

public class ScryfallCardPrices
{
    public string ScryfallId { get; set; } // Could this be saved as a guid?
    public double? Price { get; set; }
    public double? PriceFoil { get; set; }
    public double? TixPrice { get; set; }
    // public DateTime Created { get; set; }
}