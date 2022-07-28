using Carpentry.Logic;

namespace Carpentry.UI.Models;

public class TrackedSetDto
{
    public static TrackedSetDto FromModel(TrackedSetDetail props)
    {
        return new TrackedSetDto()
        {
            SetId = props.SetId,
            Code = props.Code,
            Name = props.Name,
            
            LastUpdated = props.DataLastUpdated.ToString(),
        };
    }
    
    public int SetId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    
    // public int OwnedCount { get; set; }
    // public int CollectedCount { get; set; }
    
    //total cards in a set
    // "SetCount" | "SetTotal"
    // public int TotalCount { get; set; } 
    
    // public bool IsTracked { get; set; }
    
    public bool CanBeAdded { get; set; }
    public bool CanBeUpdated { get; set; }
    public bool CanBeRemoved { get; set; }
    public string LastUpdated { get; set; }
    
}