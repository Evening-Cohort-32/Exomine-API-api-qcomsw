namespace ExomineAPI.Models.DTOs;

public class GovernorDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "active";
    public int ColonyId { get; set; }
    public string? ColonyName { get; set; }
}
