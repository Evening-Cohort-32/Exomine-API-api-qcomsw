namespace ExomineAPI.Models.DTOs;

public class MiningFacilityDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "active";
    public List<FacilityInventoryDTO> Inventory { get; set; } = new();
}
