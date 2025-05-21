using APBD999999;
using LogicPart.Data;
using LogicPart.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApbdContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/devices", async (ApbdContext dbContext, CancellationToken ct) =>
{
    var devices = await dbContext.Devices
        .Select(d => new DeviceListDto
        {
            Id = d.Id,
            Name = d.Name
        })
        .ToListAsync(ct);

    return Results.Ok(devices);
}).WithOpenApi();

app.MapGet("/api/devices/{id}", async (int id, ApbdContext dbContext, CancellationToken ct) =>
{
    var device = await dbContext.Devices
        .Include(d => d.DeviceType)
        .Include(d => d.DeviceEmployees.Where(de => de.ReturnDate == null))
            .ThenInclude(de => de.Employee)
                .ThenInclude(e => e.Person)
        .FirstOrDefaultAsync(d => d.Id == id, ct);

    if (device is null) return Results.NotFound();

    var dto = new DeviceDetailsDto
    {
        Name = device.Name,
        DeviceTypeName = device.DeviceType!.Name,
        IsEnabled = device.IsEnabled,
        AdditionalProperties = device.AdditionalProperties,
        CurrentEmployee = device.DeviceEmployees.FirstOrDefault() is { } de
            ? new CurrentEmployeeDto
              {
                  Id = de.EmployeeId,
                  FullName = $"{de.Employee.Person.FirstName} {de.Employee.Person.LastName}"
              }
            : null
    };

    return Results.Ok(dto);
}).WithOpenApi();

app.MapPost("/api/devices", async ([FromBody] CreateDeviceDto dto, ApbdContext dbContext, CancellationToken ct) =>
{
    var deviceType = await dbContext.DeviceTypes.FirstOrDefaultAsync(dt => dt.Name == dto.DeviceTypeName, ct);
    if (deviceType == null) return Results.BadRequest($"Device type '{dto.DeviceTypeName}' not found.");

    var device = new Device
    {
        Name = dto.Name,
        IsEnabled = dto.IsEnabled,
        AdditionalProperties = dto.AdditionalProperties.ToString() ?? string.Empty,
        DeviceTypeId = deviceType.Id
    };

    dbContext.Devices.Add(device);
    await dbContext.SaveChangesAsync(ct);

    return Results.Created($"/api/devices/{device.Id}", new { device.Id });
}).WithOpenApi();

app.MapGet("/api/employees", async (ApbdContext dbContext, CancellationToken ct) =>
{
    var employees = await dbContext.Employees
        .Include(e => e.Person)
        .Select(e => new EmployeeListDto
        {
            Id = e.Id,
            FullName = $"{e.Person.FirstName} {e.Person.LastName}"
        })
        .ToListAsync(ct);

    return Results.Ok(employees);
}).WithOpenApi();

app.MapGet("/api/employees/{id}", async (int id, ApbdContext dbContext, CancellationToken ct) =>
{
    var employee = await dbContext.Employees
        .Include(e => e.Person)
        .Include(e => e.Position)
        .FirstOrDefaultAsync(e => e.Id == id, ct);

    if (employee is null) return Results.NotFound();

    var dto = new EmployeeDetailsDto
    {
        PassportNumber = employee.Person.PassportNumber,
        FirstName = employee.Person.FirstName,
        MiddleName = employee.Person.MiddleName,
        LastName = employee.Person.LastName,
        PhoneNumber = employee.Person.PhoneNumber,
        Email = employee.Person.Email,
        Salary = employee.Salary,
        HireDate = employee.HireDate,
        Position = new PositionDto
        {
            Id = employee.Position.Id,
            Name = employee.Position.Name
        }
    };

    return Results.Ok(dto);
}).WithOpenApi();

app.MapPost("/api/employees", async ([FromBody] CreateEmployeeDto dto, ApbdContext dbContext, CancellationToken ct) =>
{
    var position = await dbContext.Positions.FirstOrDefaultAsync(p => p.Id == dto.PositionId, ct);
    if (position == null) return Results.BadRequest("Invalid PositionId.");

    var person = new Person
    {
        FirstName = dto.FirstName,
        MiddleName = dto.MiddleName,
        LastName = dto.LastName,
        PassportNumber = dto.PassportNumber,
        PhoneNumber = dto.PhoneNumber,
        Email = dto.Email
    };

    var employee = new Employee
    {
        Salary = dto.Salary,
        Position = position,
        Person = person,
        HireDate = dto.HireDate
    };

    dbContext.Employees.Add(employee);
    await dbContext.SaveChangesAsync(ct);

    return Results.Created($"/api/employees/{employee.Id}", new { employee.Id });
}).WithOpenApi();

app.Run();