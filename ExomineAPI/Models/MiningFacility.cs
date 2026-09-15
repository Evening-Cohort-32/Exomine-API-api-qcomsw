namespace ExomineAPI.Models;

public class MiningFacility
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
