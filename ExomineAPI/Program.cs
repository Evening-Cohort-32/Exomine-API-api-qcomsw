using ExomineAPI.Models;
using ExomineAPI.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new ApiErrorDTO { Error = "An unexpected error occurred." });
    });
});

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

List<Transaction> transactions = new()
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

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------
IResult Error(int statusCode, string message) =>
    Results.Json(new ApiErrorDTO { Error = message }, statusCode: statusCode);

// ---------------------------------------------------------------------------
//Governor endpoints
// ---------------------------------------------------------------------------
app.MapGet("/api/governors", (bool? active) =>
{
    var query = governors.AsEnumerable();
    if (active is not null)
    {
        query = query.Where(g => g.Status == active.Value);
    }
    return Results.Ok(query.Select(g => new GovernorDTO
    {
        Id = g.Id,
        Name = g.Name,
        ColonyId = g.ColonyId,
        Status = g.Status
    }));
});

app.MapGet("/api/governors/{id}", (int id) =>
{
    Governor? governor = governors.FirstOrDefault(g => g.Id == id);
    if (governor is null)
    {
        return Error(404, $"Governor with id {id} not found.");
    }
    return Results.Ok(new GovernorDTO
    {
        Id = governor.Id,
        Name = governor.Name,
        ColonyId = governor.ColonyId,
        Status = governor.Status
    });
});

app.MapPost("/api/governors", (Governor governor) =>
{
    if (string.IsNullOrWhiteSpace(governor.Name))
    {
        return Error(400, "Governor name is required.");
    }

    Colony? colony = colonies.FirstOrDefault(c => c.Id == governor.ColonyId);
    if (colony is null)
    {
        return Error(400, $"Colony with id {governor.ColonyId} does not exist.");
    }

    governor.Id = governors.Count == 0 ? 1 : governors.Max(g => g.Id) + 1;
    governors.Add(governor);

    return Results.Created($"/api/governors/{governor.Id}", new GovernorDTO
    {
        Id = governor.Id,
        Name = governor.Name,
        ColonyId = governor.ColonyId,
        Status = governor.Status
    });
});

app.MapPut("/api/governors/{id}", (int id, Governor governor) =>
{
    if (id != governor.Id)
    {
        return Error(400, "The id in the route must match the id in the request body.");
    }
    if (string.IsNullOrWhiteSpace(governor.Name))
    {
        return Error(400, "Governor name is required.");
    }

    Governor? governorToUpdate = governors.FirstOrDefault(g => g.Id == id);
    if (governorToUpdate is null)
    {
        return Error(404, $"Governor with id {id} not found.");
    }

    Colony? colony = colonies.FirstOrDefault(c => c.Id == governor.ColonyId);
    if (colony is null)
    {
        return Error(400, $"Colony with id {governor.ColonyId} does not exist.");
    }

    //status change is recorded automatically, never a separate client request
    if (governorToUpdate.Status != governor.Status)
    {
        governorHistories.Add(new GovernorHistory
        {
            Id = governorHistories.Count == 0 ? 1 : governorHistories.Max(g => g.Id) + 1,
            GovernorId = governorToUpdate.Id,
            ColonyId = governorToUpdate.ColonyId,
            PreviousStatus = governorToUpdate.Status,
            Timestamp = DateTime.UtcNow,
        });
    }

    governorToUpdate.Name = governor.Name;
    governorToUpdate.ColonyId = governor.ColonyId;
    governorToUpdate.Status = governor.Status;

    return Results.NoContent();
});

app.MapDelete("/api/governors/{id}", (int id) =>
{
    Governor? governorToRemove = governors.FirstOrDefault(g => g.Id == id);
    if (governorToRemove is null)
    {
        return Error(404, $"Governor with id {id} not found.");
    }

    governors.Remove(governorToRemove);
    return Results.NoContent();
});

// ---------------------------------------------------------------------------
// GovernorHistory endpoints
// ---------------------------------------------------------------------------
app.MapGet("/api/governorhistories", () =>
{
    return governorHistories.Select(gh => new GovernorHistoryDTO
    {
        Id = gh.Id,
        GovernorId = gh.GovernorId,
        GovernorName = governors.FirstOrDefault(g => g.Id == gh.GovernorId)?.Name,
        ColonyId = gh.ColonyId,
        ColonyName = colonies.FirstOrDefault(c => c.Id == gh.ColonyId)?.Name,
        PreviousStatus = gh.PreviousStatus,
        Timestamp = gh.Timestamp
    });
});

app.MapGet("/api/governorhistories/{id}", (int id) =>
{
    GovernorHistory? governorHistory = governorHistories.FirstOrDefault(gh => gh.Id == id);
    if (governorHistory is null)
    {
        return Error(404, $"Governor history with id {id} not found.");
    }

    return Results.Ok(new GovernorHistoryDTO
    {
        Id = governorHistory.Id,
        GovernorId = governorHistory.GovernorId,
        GovernorName = governors.FirstOrDefault(g => g.Id == governorHistory.GovernorId)?.Name,
        ColonyId = governorHistory.ColonyId,
        ColonyName = colonies.FirstOrDefault(c => c.Id == governorHistory.ColonyId)?.Name,
        PreviousStatus = governorHistory.PreviousStatus,
        Timestamp = governorHistory.Timestamp
    });
});

// ---------------------------------------------------------------------------
//Colony endpoints
// ---------------------------------------------------------------------------

app.MapGet("/api/colonies", () =>
{
    return colonies.Select(c => new ColonyDTO
    {
        Id = c.Id,
        Name = c.Name,
        Location = c.Location
    });
});

app.MapGet("/api/colonies/{id}", (int id) =>
{
    Colony colony = colonies.FirstOrDefault(c => c.Id == id);
    //check if id is valid
    if (colony == null) { return Results.NotFound(); }

    return Results.Ok(new ColonyDTO
    {
        Id = colony.Id,
        Name = colony.Name,
        Location = colony.Location,
        Inventory = colonyInventories.Where(ci => ci.ColonyId == colony.Id)
        .Select(ci => new ColonyInventoryDTO
        {
            Id = ci.Id,
            ColonyId = ci.ColonyId,
            MineralId = ci.MineralId,
            MineralName = minerals.Where(m => m.Id == ci.MineralId)
            .Select(m => m.Name).First(),
            Quantity = ci.Quantity
        }).ToList()
    });
});

app.MapPost("/api/colonies/", (Colony colony) =>
{
    //create id for new colony
    colony.Id = colonies.Max(c => c.Id) + 1;

    //add new colony to database
    colonies.Add(colony);

    return Results.Created($"/api/colonies/{colony.Id}", new ColonyDTO
    {
        Id = colony.Id,
        Name = colony.Name,
        Location = colony.Location
    });
});

app.MapPut("/api/colonies/{id}", (int id, Colony colony) =>
{
    //check if colony is valid
    Colony colonyToUpdate = colonies.FirstOrDefault(c => c.Id == id);
    if (colony.Id != id || colonyToUpdate == null) { return Results.BadRequest(); }

    //update colony in database
    colonies[id - 1] = colony;
    return Results.NoContent();
});

app.MapDelete("/api/colonies/{id}", (int id) =>
{
    //check if id is valid
    Colony colonyToRemove = colonies.FirstOrDefault(c => c.Id == id);
    if (colonyToRemove == null) { return Results.NotFound(); }

    //remove colony from database
    else colonies.Remove(colonyToRemove);
    return Results.NoContent();
});

// ---------------------------------------------------------------------------
//Transaction endpoints
// ---------------------------------------------------------------------------

app.MapGet("/api/transactions", () =>
{
    return transactions.Select(t => new TransactionDTO
    {
        Id = t.Id,
        GovernorId = t.GovernorId,
        GovernorName = governors.Where(g => g.Id == t.GovernorId).Select(g => g.Name).First(),
        ColonyId = t.ColonyId,
        ColonyName = colonies.Where(c => c.Id == t.ColonyId).Select(c => c.Name).First(),
        MineralId = t.MineralId,
        MineralName = minerals.Where(m => m.Id == t.MineralId).Select(m => m.Name).First(),
        MiningFacilityId = t.MiningFacilityId,
        FacilityName = miningFacilities.Where(mf => mf.Id == t.MiningFacilityId).Select(mf => mf.Name).First(),
        Quantity = t.Quantity,
        Timestamp = t.Timestamp
    });
});

app.MapGet("/api/transactions/{id}", (int id) =>
{
    Transaction transaction = transactions.FirstOrDefault(t => t.Id == id);
    //check if transaction is valid
    if (transaction == null)
    {
        return Error(404, $"Transaction with id {id} not found.");
    }

    return Results.Ok(new TransactionDTO
    {
        Id = transaction.Id,
        GovernorId = transaction.GovernorId,
        GovernorName = governors.Where(g => g.Id == transaction.GovernorId).Select(g => g.Name).First(),
        ColonyId = transaction.ColonyId,
        ColonyName = colonies.Where(c => c.Id == transaction.ColonyId).Select(c => c.Name).First(),
        MineralId = transaction.MineralId,
        MineralName = minerals.Where(m => m.Id == transaction.MineralId).Select(m => m.Name).First(),
        MiningFacilityId = transaction.MiningFacilityId,
        FacilityName = miningFacilities.Where(mf => mf.Id == transaction.MiningFacilityId).Select(mf => mf.Name).First(),
        Quantity = transaction.Quantity,
        Timestamp = transaction.Timestamp
    });
});


app.Run();