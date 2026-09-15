using ExomineAPI.Models;
using ExomineAPI.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ---------------------------------------------------------------------------
// In-memory "database"
// ---------------------------------------------------------------------------

List<Colony> colonies = new()
{
    new Colony { Id = 1, Name = "New Shanghai", Location = "Mars" },
    new Colony { Id = 2, Name = "Frostgate", Location = "Europa" },
    new Colony { Id = 3, Name = "Red Dust Outpost", Location = "Mars" },
};

List<Governor> governors = new()
{
    new Governor { Id = 1, Name = "Aria Chen", ColonyId = 1, Status = true },
    new Governor { Id = 2, Name = "Marcus Webb", ColonyId = 2, Status = true },
    new Governor { Id = 3, Name = "Elena Petrov", ColonyId = 3, Status = false },
    new Governor { Id = 4, Name = "Sam Osei", ColonyId = 1, Status = true },
};

List<MiningFacility> miningFacilities = new()
{
    new MiningFacility { Id = 1, Name = "Olympus Mons Extraction", IsActive = true },
    new MiningFacility { Id = 2, Name = "Europa Ice Rig", IsActive = true },
    new MiningFacility { Id = 3, Name = "Deimos Quarry", IsActive = false },
};

List<Mineral> minerals = new()
{
    new Mineral { Id = 1, Name = "Iron" },
    new Mineral { Id = 2, Name = "Magnesium" },
    new Mineral { Id = 3, Name = "Titanium" },
    new Mineral { Id = 4, Name = "Water Ice" },
    new Mineral { Id = 5, Name = "Palladium" },
};

List<FacilityInventory> facilityInventories = new()
{
    new FacilityInventory { Id = 1, MiningFacilityId = 1, MineralId = 1, Quantity = 500 },
    new FacilityInventory { Id = 2, MiningFacilityId = 1, MineralId = 2, Quantity = 300 },
    new FacilityInventory { Id = 3, MiningFacilityId = 1, MineralId = 3, Quantity = 0 },
    new FacilityInventory { Id = 4, MiningFacilityId = 2, MineralId = 4, Quantity = 1000 },
    new FacilityInventory { Id = 5, MiningFacilityId = 2, MineralId = 2, Quantity = 50 },
    new FacilityInventory { Id = 6, MiningFacilityId = 3, MineralId = 1, Quantity = 200 },
    new FacilityInventory { Id = 7, MiningFacilityId = 3, MineralId = 5, Quantity = 10 },
};

List<ColonyInventory> colonyInventories = new()
{
    new ColonyInventory { Id = 1, ColonyId = 1, MineralId = 1, Quantity = 50 },
    new ColonyInventory { Id = 2, ColonyId = 1, MineralId = 2, Quantity = 20 },
    new ColonyInventory { Id = 3, ColonyId = 2, MineralId = 4, Quantity = 200 },
    new ColonyInventory { Id = 4, ColonyId = 3, MineralId = 1, Quantity = 10 },
};

List<GovernorHistory> governorHistories = new()
{
    new GovernorHistory
    {
        Id = 1,
        GovernorId = 3,
        ColonyId = 3,
        PreviousStatus = true,
        Timestamp = new DateTime(2026, 3, 4, 0, 0, 0, DateTimeKind.Utc),
    },
};

List<Transaction> transactions = new()
{
    new Transaction
    {
        Id = 1,
        ColonyId = 1,
        MiningFacilityId = 1,
        MineralId = 1,
        Quantity = 1,
        Timestamp = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc),
    },
};

int nextColonyId = colonies.Max(c => c.Id) + 1;
int nextGovernorId = governors.Max(g => g.Id) + 1;
int nextMiningFacilityId = miningFacilities.Max(f => f.Id) + 1;
int nextMineralId = minerals.Max(m => m.Id) + 1;
int nextFacilityInventoryId = facilityInventories.Max(fi => fi.Id) + 1;
int nextColonyInventoryId = colonyInventories.Max(ci => ci.Id) + 1;
int nextGovernorHistoryId = governorHistories.Max(gh => gh.Id) + 1;
int nextTransactionId = transactions.Max(t => t.Id) + 1;

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

IResult Error(int statusCode, string message) =>
    Results.Json(new ApiErrorDTO { Error = message }, statusCode: statusCode);

GovernorDTO GovernorToDTO(Governor governor) => new()
{
    Id = governor.Id,
    Name = governor.Name,
    Status = governor.Status,
    ColonyId = governor.ColonyId,
    ColonyName = colonies.FirstOrDefault(c => c.Id == governor.ColonyId)?.Name,
};

ColonyInventoryDTO ColonyInventoryToDTO(ColonyInventory inventory) => new()
{
    Id = inventory.Id,
    ColonyId = inventory.ColonyId,
    MineralId = inventory.MineralId,
    MineralName = minerals.FirstOrDefault(m => m.Id == inventory.MineralId)?.Name,
    Quantity = inventory.Quantity,
};

FacilityInventoryDTO FacilityInventoryToDTO(FacilityInventory inventory) => new()
{
    Id = inventory.Id,
    MiningFacilityId = inventory.MiningFacilityId,
    MineralId = inventory.MineralId,
    MineralName = minerals.FirstOrDefault(m => m.Id == inventory.MineralId)?.Name,
    Quantity = inventory.Quantity,
};

ColonyDTO ColonyToDTO(Colony colony) => new()
{
    Id = colony.Id,
    Name = colony.Name,
    Location = colony.Location,
    Inventory = colonyInventories
        .Where(ci => ci.ColonyId == colony.Id)
        .Select(ColonyInventoryToDTO)
        .ToList(),
};

MiningFacilityDTO MiningFacilityToDTO(MiningFacility facility) => new()
{
    Id = facility.Id,
    Name = facility.Name,
    IsActive = facility.IsActive,
    Inventory = facilityInventories
        .Where(fi => fi.MiningFacilityId == facility.Id)
        .Select(FacilityInventoryToDTO)
        .ToList(),
};

MineralDTO MineralToDTO(Mineral mineral) => new() { Id = mineral.Id, Name = mineral.Name };

GovernorHistoryDTO GovernorHistoryToDTO(GovernorHistory history) => new()
{
    Id = history.Id,
    GovernorId = history.GovernorId,
    GovernorName = governors.FirstOrDefault(g => g.Id == history.GovernorId)?.Name,
    ColonyId = history.ColonyId,
    ColonyName = colonies.FirstOrDefault(c => c.Id == history.ColonyId)?.Name,
    PreviousStatus = history.PreviousStatus,
    Timestamp = history.Timestamp,
};

TransactionDTO TransactionToDTO(Transaction transaction) => new()
{
    Id = transaction.Id,
    ColonyId = transaction.ColonyId,
    ColonyName = colonies.FirstOrDefault(c => c.Id == transaction.ColonyId)?.Name,
    MiningFacilityId = transaction.MiningFacilityId,
    FacilityName = miningFacilities.FirstOrDefault(f => f.Id == transaction.MiningFacilityId)?.Name,
    MineralId = transaction.MineralId,
    MineralName = minerals.FirstOrDefault(m => m.Id == transaction.MineralId)?.Name,
    Quantity = transaction.Quantity,
    Timestamp = transaction.Timestamp,
};

// ---------------------------------------------------------------------------
// Governors
// ---------------------------------------------------------------------------

