namespace ExomineAPI.Models;

public class Governor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ColonyId { get; set; }
    public bool Status { get; set; } = true;
}
