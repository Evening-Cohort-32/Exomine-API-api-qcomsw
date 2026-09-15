namespace ExomineAPI.Models.DTOs;

public class TransactionDTO
{
    public int Id { get; set; }
    public int GovernorId { get; set; }
    public string? GovernorName { get; set; }
    public int ColonyId { get; set; }
    public string? ColonyName { get; set; }
    public int MiningFacilityId { get; set; }
    public string? FacilityName { get; set; }
    public int MineralId { get; set; }
    public string? MineralName { get; set; }
    public int Quantity { get; set; }
    public DateTime Timestamp { get; set; }
}
