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

app.MapGet("/api/governors", (bool? active) =>
{
    var query = governors.AsEnumerable();
    if (active is not null)
    {
        query = query.Where(g => g.Status == active.Value);
    }
    return Results.Ok(query.Select(GovernorToDTO));
});

app.MapGet("/api/governors/{id}", (int id) =>
{
    var governor = governors.FirstOrDefault(g => g.Id == id);
    return governor is null ? Error(404, $"Governor with id {id} not found.") : Results.Ok(GovernorToDTO(governor));
});

app.MapPost("/api/governors", (GovernorDTO dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Error(400, "Governor name is required.");
    }
    if (colonies.All(c => c.Id != dto.ColonyId))
    {
        return Error(400, $"Colony with id {dto.ColonyId} does not exist.");
    }

    var governor = new Governor
    {
        Id = nextGovernorId++,
        Name = dto.Name,
        ColonyId = dto.ColonyId,
        Status = dto.Status,
    };
    governors.Add(governor);
    return Results.Created($"/api/governors/{governor.Id}", GovernorToDTO(governor));
});

app.MapPut("/api/governors/{id}", (int id, GovernorDTO dto) =>
{
    var governor = governors.FirstOrDefault(g => g.Id == id);
    if (governor is null)
    {
        return Error(404, $"Governor with id {id} not found.");
    }
    if (colonies.All(c => c.Id != dto.ColonyId))
    {
        return Error(400, $"Colony with id {dto.ColonyId} does not exist.");
    }

    if (governor.Status != dto.Status)
    {
        governorHistories.Add(new GovernorHistory
        {
            Id = nextGovernorHistoryId++,
            GovernorId = governor.Id,
            ColonyId = governor.ColonyId,
            PreviousStatus = governor.Status,
            Timestamp = DateTime.UtcNow,
        });
    }

    governor.Name = dto.Name;
    governor.ColonyId = dto.ColonyId;
    governor.Status = dto.Status;
    return Results.Ok(GovernorToDTO(governor));
});

app.MapDelete("/api/governors/{id}", (int id) =>
{
    var governor = governors.FirstOrDefault(g => g.Id == id);
    if (governor is null)
    {
        return Error(404, $"Governor with id {id} not found.");
    }
    governors.Remove(governor);
    return Results.NoContent();
});

// ---------------------------------------------------------------------------
// Colonies
// ---------------------------------------------------------------------------

app.MapGet("/api/colonies", () => Results.Ok(colonies.Select(ColonyToDTO)));

app.MapGet("/api/colonies/{id}", (int id) =>
{
    var colony = colonies.FirstOrDefault(c => c.Id == id);
    return colony is null ? Error(404, $"Colony with id {id} not found.") : Results.Ok(ColonyToDTO(colony));
});

app.MapPost("/api/colonies", (ColonyDTO dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Location))
    {
        return Error(400, "Colony name and location are required.");
    }

    var colony = new Colony { Id = nextColonyId++, Name = dto.Name, Location = dto.Location };
    colonies.Add(colony);
    return Results.Created($"/api/colonies/{colony.Id}", ColonyToDTO(colony));
});

app.MapPut("/api/colonies/{id}", (int id, ColonyDTO dto) =>
{
    var colony = colonies.FirstOrDefault(c => c.Id == id);
    if (colony is null)
    {
        return Error(404, $"Colony with id {id} not found.");
    }
    colony.Name = dto.Name;
    colony.Location = dto.Location;
    return Results.Ok(ColonyToDTO(colony));
});

app.MapDelete("/api/colonies/{id}", (int id) =>
{
    var colony = colonies.FirstOrDefault(c => c.Id == id);
    if (colony is null)
    {
        return Error(404, $"Colony with id {id} not found.");
    }
    colonies.Remove(colony);
    return Results.NoContent();
});

// ---------------------------------------------------------------------------
// Mining Facilities
// ---------------------------------------------------------------------------

app.MapGet("/api/miningfacilities", (bool? active) =>
{
    var query = miningFacilities.AsEnumerable();
    if (active is not null)
    {
        query = query.Where(f => f.IsActive == active.Value);
    }
    return Results.Ok(query.Select(MiningFacilityToDTO));
});

app.MapGet("/api/miningfacilities/{id}", (int id) =>
{
    var facility = miningFacilities.FirstOrDefault(f => f.Id == id);
    return facility is null ? Error(404, $"Mining facility with id {id} not found.") : Results.Ok(MiningFacilityToDTO(facility));
});

app.MapPost("/api/miningfacilities", (MiningFacilityDTO dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Error(400, "Mining facility name is required.");
    }

    var facility = new MiningFacility
    {
        Id = nextMiningFacilityId++,
        Name = dto.Name,
        IsActive = dto.IsActive,
    };
    miningFacilities.Add(facility);
    return Results.Created($"/api/miningfacilities/{facility.Id}", MiningFacilityToDTO(facility));
});

app.MapPut("/api/miningfacilities/{id}", (int id, MiningFacilityDTO dto) =>
{
    var facility = miningFacilities.FirstOrDefault(f => f.Id == id);
    if (facility is null)
    {
        return Error(404, $"Mining facility with id {id} not found.");
    }
    facility.Name = dto.Name;
    facility.IsActive = dto.IsActive;
    return Results.Ok(MiningFacilityToDTO(facility));
});

app.MapDelete("/api/miningfacilities/{id}", (int id) =>
{
    var facility = miningFacilities.FirstOrDefault(f => f.Id == id);
    if (facility is null)
    {
        return Error(404, $"Mining facility with id {id} not found.");
    }
    miningFacilities.Remove(facility);
    return Results.NoContent();
});

// ---------------------------------------------------------------------------
// Minerals
// ---------------------------------------------------------------------------

app.MapGet("/api/minerals", () => Results.Ok(minerals.Select(MineralToDTO)));

app.MapGet("/api/minerals/{id}", (int id) =>
{
    var mineral = minerals.FirstOrDefault(m => m.Id == id);
    return mineral is null ? Error(404, $"Mineral with id {id} not found.") : Results.Ok(MineralToDTO(mineral));
});

app.MapPost("/api/minerals", (MineralDTO dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Error(400, "Mineral name is required.");
    }
    if (minerals.Any(m => string.Equals(m.Name, dto.Name, StringComparison.OrdinalIgnoreCase)))
    {
        return Error(409, $"A mineral named '{dto.Name}' already exists.");
    }

    var mineral = new Mineral { Id = nextMineralId++, Name = dto.Name };
    minerals.Add(mineral);
    return Results.Created($"/api/minerals/{mineral.Id}", MineralToDTO(mineral));
});

app.MapPut("/api/minerals/{id}", (int id, MineralDTO dto) =>
{
    var mineral = minerals.FirstOrDefault(m => m.Id == id);
    if (mineral is null)
    {
        return Error(404, $"Mineral with id {id} not found.");
    }
    if (minerals.Any(m => m.Id != id && string.Equals(m.Name, dto.Name, StringComparison.OrdinalIgnoreCase)))
    {
        return Error(409, $"A mineral named '{dto.Name}' already exists.");
    }
    mineral.Name = dto.Name;
    return Results.Ok(MineralToDTO(mineral));
});

app.MapDelete("/api/minerals/{id}", (int id) =>
{
    var mineral = minerals.FirstOrDefault(m => m.Id == id);
    if (mineral is null)
    {
        return Error(404, $"Mineral with id {id} not found.");
    }
    minerals.Remove(mineral);
    return Results.NoContent();
});

// ---------------------------------------------------------------------------
// Colony Inventory
// ---------------------------------------------------------------------------

app.MapGet("/api/colonyinventory", () => Results.Ok(colonyInventories.Select(ColonyInventoryToDTO)));

app.MapGet("/api/colonyinventory/{id}", (int id) =>
{
    var inventory = colonyInventories.FirstOrDefault(ci => ci.Id == id);
    return inventory is null ? Error(404, $"Colony inventory with id {id} not found.") : Results.Ok(ColonyInventoryToDTO(inventory));
});

app.MapPost("/api/colonyinventory", (ColonyInventoryDTO dto) =>
{
    if (colonies.All(c => c.Id != dto.ColonyId))
    {
        return Error(400, $"Colony with id {dto.ColonyId} does not exist.");
    }
    if (minerals.All(m => m.Id != dto.MineralId))
    {
        return Error(400, $"Mineral with id {dto.MineralId} does not exist.");
    }
    if (dto.Quantity < 0)
    {
        return Error(400, "Quantity cannot be negative.");
    }

    var inventory = new ColonyInventory
    {
        Id = nextColonyInventoryId++,
        ColonyId = dto.ColonyId,
        MineralId = dto.MineralId,
        Quantity = dto.Quantity,
    };
    colonyInventories.Add(inventory);
    return Results.Created($"/api/colonyinventory/{inventory.Id}", ColonyInventoryToDTO(inventory));
});

app.MapPut("/api/colonyinventory/{id}", (int id, ColonyInventoryDTO dto) =>
{
    var inventory = colonyInventories.FirstOrDefault(ci => ci.Id == id);
    if (inventory is null)
    {
        return Error(404, $"Colony inventory with id {id} not found.");
    }
    if (dto.Quantity < 0)
    {
        return Error(400, "Quantity cannot be negative.");
    }
    inventory.ColonyId = dto.ColonyId;
    inventory.MineralId = dto.MineralId;
    inventory.Quantity = dto.Quantity;
    return Results.Ok(ColonyInventoryToDTO(inventory));
});

app.MapDelete("/api/colonyinventory/{id}", (int id) =>
{
    var inventory = colonyInventories.FirstOrDefault(ci => ci.Id == id);
    if (inventory is null)
    {
        return Error(404, $"Colony inventory with id {id} not found.");
    }
    colonyInventories.Remove(inventory);
    return Results.NoContent();
});

// ---------------------------------------------------------------------------
// Facility Inventory
// ---------------------------------------------------------------------------

app.MapGet("/api/facilityinventory", () => Results.Ok(facilityInventories.Select(FacilityInventoryToDTO)));

app.MapGet("/api/facilityinventory/{id}", (int id) =>
{
    var inventory = facilityInventories.FirstOrDefault(fi => fi.Id == id);
    return inventory is null ? Error(404, $"Facility inventory with id {id} not found.") : Results.Ok(FacilityInventoryToDTO(inventory));
});

app.MapPost("/api/facilityinventory", (FacilityInventoryDTO dto) =>
{
    if (miningFacilities.All(f => f.Id != dto.MiningFacilityId))
    {
        return Error(400, $"Mining facility with id {dto.MiningFacilityId} does not exist.");
    }
    if (minerals.All(m => m.Id != dto.MineralId))
    {
        return Error(400, $"Mineral with id {dto.MineralId} does not exist.");
    }
    if (dto.Quantity < 0)
    {
        return Error(400, "Quantity cannot be negative.");
    }

    var inventory = new FacilityInventory
    {
        Id = nextFacilityInventoryId++,
        MiningFacilityId = dto.MiningFacilityId,
        MineralId = dto.MineralId,
        Quantity = dto.Quantity,
    };
    facilityInventories.Add(inventory);
    return Results.Created($"/api/facilityinventory/{inventory.Id}", FacilityInventoryToDTO(inventory));
});

app.MapPut("/api/facilityinventory/{id}", (int id, FacilityInventoryDTO dto) =>
{
    var inventory = facilityInventories.FirstOrDefault(fi => fi.Id == id);
    if (inventory is null)
    {
        return Error(404, $"Facility inventory with id {id} not found.");
    }
    if (dto.Quantity < 0)
    {
        return Error(400, "Quantity cannot be negative.");
    }
    inventory.MiningFacilityId = dto.MiningFacilityId;
    inventory.MineralId = dto.MineralId;
    inventory.Quantity = dto.Quantity;
    return Results.Ok(FacilityInventoryToDTO(inventory));
});

app.MapDelete("/api/facilityinventory/{id}", (int id) =>
{
    var inventory = facilityInventories.FirstOrDefault(fi => fi.Id == id);
    if (inventory is null)
    {
        return Error(404, $"Facility inventory with id {id} not found.");
    }
    facilityInventories.Remove(inventory);
    return Results.NoContent();
});

// ---------------------------------------------------------------------------
// Governor History (read-only)
// ---------------------------------------------------------------------------

app.MapGet("/api/governorhistory", (int? governorId) =>
{
    var query = governorHistories.AsEnumerable();
    if (governorId is not null)
    {
        query = query.Where(gh => gh.GovernorId == governorId);
    }
    return Results.Ok(query.Select(GovernorHistoryToDTO));
});

app.MapGet("/api/governorhistory/{id}", (int id) =>
{
    var history = governorHistories.FirstOrDefault(gh => gh.Id == id);
    return history is null ? Error(404, $"Governor history record with id {id} not found.") : Results.Ok(GovernorHistoryToDTO(history));
});

// ---------------------------------------------------------------------------
// Transactions (read-only)
// ---------------------------------------------------------------------------

app.MapGet("/api/transactions", (int? colonyId, int? miningFacilityId, int? mineralId, DateTime? startDate, DateTime? endDate) =>
{
    var query = transactions.AsEnumerable();
    if (colonyId is not null)
    {
        query = query.Where(t => t.ColonyId == colonyId);
    }
    if (miningFacilityId is not null)
    {
        query = query.Where(t => t.MiningFacilityId == miningFacilityId);
    }
    if (mineralId is not null)
    {
        query = query.Where(t => t.MineralId == mineralId);
    }
    if (startDate is not null)
    {
        query = query.Where(t => t.Timestamp >= startDate);
    }
    if (endDate is not null)
    {
        query = query.Where(t => t.Timestamp <= endDate);
    }
    return Results.Ok(query.Select(TransactionToDTO));
});

app.MapGet("/api/transactions/{id}", (int id) =>
{
    var transaction = transactions.FirstOrDefault(t => t.Id == id);
    return transaction is null ? Error(404, $"Transaction with id {id} not found.") : Results.Ok(TransactionToDTO(transaction));
});

// ---------------------------------------------------------------------------
// Purchase
// ---------------------------------------------------------------------------

app.MapPut("/api/purchase", (PurchaseRequestDTO request) =>
{
    var governor = governors.FirstOrDefault(g => g.Id == request.GovernorId);
    if (governor is null)
    {
        return Error(404, $"Governor with id {request.GovernorId} not found.");
    }

    var facility = miningFacilities.FirstOrDefault(f => f.Id == request.MiningFacilityId);
    if (facility is null)
    {
        return Error(404, $"Mining facility with id {request.MiningFacilityId} not found.");
    }

    var facilityStock = facilityInventories.FirstOrDefault(
        fi => fi.MiningFacilityId == request.MiningFacilityId && fi.MineralId == request.MineralId);
    if (facilityStock is null)
    {
        return Error(404, $"Mineral with id {request.MineralId} is not stocked at this facility.");
    }

    if (!governor.Status)
    {
        return Error(409, "Governor is not active and cannot make purchases.");
    }

    if (!facility.IsActive)
    {
        return Error(409, "Mining facility is not active and cannot fulfill purchases.");
    }

    if (facilityStock.Quantity < 1)
    {
        return Error(409, "Mineral is out of stock at this facility.");
    }

    facilityStock.Quantity -= 1;

    var colonyStock = colonyInventories.FirstOrDefault(
        ci => ci.ColonyId == governor.ColonyId && ci.MineralId == request.MineralId);
    if (colonyStock is null)
    {
        colonyStock = new ColonyInventory
        {
            Id = nextColonyInventoryId++,
            ColonyId = governor.ColonyId,
            MineralId = request.MineralId,
            Quantity = 0,
        };
        colonyInventories.Add(colonyStock);
    }
    colonyStock.Quantity += 1;

    var transaction = new Transaction
    {
        Id = nextTransactionId++,
        ColonyId = governor.ColonyId,
        MiningFacilityId = facility.Id,
        MineralId = request.MineralId,
        Quantity = 1,
        Timestamp = DateTime.UtcNow,
    };
    transactions.Add(transaction);

    return Results.Ok(TransactionToDTO(transaction));
});

app.Run();

