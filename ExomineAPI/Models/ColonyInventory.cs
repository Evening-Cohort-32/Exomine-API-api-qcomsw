namespace ExomineAPI.Models;

public class ColonyInventory
{
    public int Id { get; set; }
    public int ColonyId { get; set; }
    public int MineralId { get; set; }
    public string? MineralName { get; set; }
    public int Quantity { get; set; }
}
