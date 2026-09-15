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
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

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

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
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
