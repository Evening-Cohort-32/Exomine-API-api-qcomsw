namespace ExomineAPI.Models.DTOs;

public class FacilityInventoryDTO
{
    public int Id { get; set; }
    public int MiningFacilityId { get; set; }
    public int MineralId { get; set; }
    public string? MineralName { get; set; }
    public int Quantity { get; set; }
}
