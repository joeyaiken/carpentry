using System.ComponentModel.DataAnnotations;

namespace Carpentry.ScryfallData.DataModels;

public class AllSetsCachedData
{
    [Key]
    public int AllSetsCachedDataId { get; set; }
    public string SetTokensString { get; set; }
    public DateTime? LastUpdated { get; set; }
}