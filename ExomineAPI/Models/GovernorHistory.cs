namespace ExomineAPI.Models;

public class GovernorHistory
{
    public int Id { get; set; }
    public int GovernorId { get; set; }
    public int ColonyId { get; set; }
    public bool PreviousStatus { get; set; }
    public DateTime Timestamp { get; set; }
}
