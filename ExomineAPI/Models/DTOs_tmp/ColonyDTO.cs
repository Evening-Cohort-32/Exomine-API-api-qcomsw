namespace ExomineAPI.Models.DTOs;

public class ColonyDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<ColonyInventoryDTO> Inventory { get; set; } = new();
}
