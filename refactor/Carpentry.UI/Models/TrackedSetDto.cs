namespace Carpentry.UI.Models;

public class TrackedSetDto
{
    public int SetId { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }
    
    public int OwnedCount { get; set; }
    
    public int CollectedCount { get; set; }
    
    public string LastUpdated { get; set; }
}