namespace ExomineAPI.Models.DTOs;

public class GovernorDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
    public int ColonyId { get; set; }
    public string? ColonyName { get; set; }
}
