using System.ComponentModel.DataAnnotations;

namespace Carpentry.ScryfallData.DataModels;

public class CardPriceData
{
    [Key]
    public int CardPriceDataId { get; set; }
    public int CachedSetDataId { get; set; }
    public string ScryfallId { get; set; } // Could this be saved as a guid?
    public double? Price { get; set; }
    public double? PriceFoil { get; set; }
    public double? TixPrice { get; set; }
    public DateTime Created { get; set; }

    //annotation
    // [ForeignKey(card)]
    public virtual CachedSetData SetData { get; set; }
}