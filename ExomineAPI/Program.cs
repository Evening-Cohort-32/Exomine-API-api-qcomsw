var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    new FacilityInventory { Id = 1, MiningFacilityId = 1, MineralId = 1, Quantity = 500 },
    new FacilityInventory { Id = 2, MiningFacilityId = 1, MineralId = 2, Quantity = 300 },
    new FacilityInventory { Id = 3, MiningFacilityId = 1, MineralId = 3, Quantity = 0 },
    new FacilityInventory { Id = 4, MiningFacilityId = 2, MineralId = 4, Quantity = 1000 },
    new FacilityInventory { Id = 5, MiningFacilityId = 2, MineralId = 2, Quantity = 50 },
    new FacilityInventory { Id = 6, MiningFacilityId = 3, MineralId = 1, Quantity = 200 },
    new FacilityInventory { Id = 7, MiningFacilityId = 3, MineralId = 5, Quantity = 10 },
    new FacilityInventory { Id = 8, MiningFacilityId = 2, MineralId = 5, Quantity = 5 },
    new FacilityInventory { Id = 9, MiningFacilityId = 3, MineralId = 3, Quantity = 15 },
};

List<ColonyInventory> colonyInventories = new()
{
    new ColonyInventory { Id = 1, ColonyId = 1, MineralId = 1, Quantity = 50 },
    new ColonyInventory { Id = 2, ColonyId = 1, MineralId = 2, Quantity = 20 },
    new ColonyInventory { Id = 3, ColonyId = 2, MineralId = 4, Quantity = 200 },
    new ColonyInventory { Id = 4, ColonyId = 3, MineralId = 1, Quantity = 10 },
    new ColonyInventory { Id = 5, ColonyId = 1, MineralId = 3, Quantity = 5 },
    new ColonyInventory { Id = 6, ColonyId = 2, MineralId = 2, Quantity = 10 },
    new ColonyInventory { Id = 7, ColonyId = 2, MineralId = 5, Quantity = 2 },
    new ColonyInventory { Id = 8, ColonyId = 3, MineralId = 4, Quantity = 30 },
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
    new GovernorHistory
    {
        Id = 2,
        GovernorId = 1,
        ColonyId = 1,
        PreviousStatus = false,
        Timestamp = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc),
    },
    new GovernorHistory
    {
        Id = 3,
        GovernorId = 4,
        ColonyId = 1,
        PreviousStatus = false,
        Timestamp = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
    },
};

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    new Transaction
    {
        Id = 1,
        GovernorId = 1,
        ColonyId = 1,
        MiningFacilityId = 1,
        MineralId = 1,
        Quantity = 1,
        Timestamp = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc),
    },
    new Transaction
    {
        Id = 2,
        GovernorId = 2,
        ColonyId = 2,
        MiningFacilityId = 2,
        MineralId = 4,
        Quantity = 5,
        Timestamp = new DateTime(2026, 3, 11, 0, 0, 0, DateTimeKind.Utc),
    },
    new Transaction
    {
        Id = 3,
        GovernorId = 4,
        ColonyId = 1,
        MiningFacilityId = 1,
        MineralId = 2,
        Quantity = 2,
        Timestamp = new DateTime(2026, 3, 12, 0, 0, 0, DateTimeKind.Utc),
    },
    new Transaction
    {
        // before Elena's 3/4 status change, so she was still active for this purchase.
        Id = 4,
        GovernorId = 3,
        ColonyId = 3,
        MiningFacilityId = 3,
        MineralId = 1,
        Quantity = 1,
        Timestamp = new DateTime(2026, 2, 20, 0, 0, 0, DateTimeKind.Utc),
    },
    new Transaction
    {
        Id = 5,
        GovernorId = 2,
        ColonyId = 2,
        MiningFacilityId = 2,
        MineralId = 5,
        Quantity = 1,
        Timestamp = new DateTime(2026, 3, 14, 0, 0, 0, DateTimeKind.Utc),
    },
};

