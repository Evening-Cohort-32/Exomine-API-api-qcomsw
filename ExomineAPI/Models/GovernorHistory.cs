namespace ExomineAPI.Models;

public class GovernorHistory
{
    public int Id { get; set; }
    public int GovernorId { get; set; }
    public int ColonyId { get; set; }
    public string PreviousStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
