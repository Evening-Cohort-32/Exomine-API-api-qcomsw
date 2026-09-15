namespace ExomineAPI.Models.DTOs;

public class GovernorHistoryDTO
{
    public int Id { get; set; }
    public int GovernorId { get; set; }
    public string? GovernorName { get; set; }
    public int ColonyId { get; set; }
    public string? ColonyName { get; set; }
    public string PreviousStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
