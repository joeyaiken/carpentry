using System.ComponentModel.DataAnnotations;

namespace Carpentry.ScryfallData.DataModels;

public class CachedSetData
{
    [Key]
    public int CachedSetDataId { get; set; }
    public string Code { get; set; }
    public string SetTokenString { get; set; }
    public string CardTokensString { get; set; }
    public DateTime LastUpdated { get; set; }
}