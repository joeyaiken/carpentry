namespace Carpentry.Data.Models;

public class CoreDefinitionUpdateHistoryData
{
    //id
    // public int CoreDefinitionUpdateHistoryDataId { get; set; }
    public int Id { get; set; }
    
    //Type "AvailableSets"
    public string Type { get; set; }
    
    //LastUpdated | UpdatedDate
    public DateTime UpdatedDate { get; set; }
}