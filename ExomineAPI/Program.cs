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
