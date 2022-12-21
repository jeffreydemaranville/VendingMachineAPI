using VendingMachineAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//implement singleton for MachineState to persist transaction data
builder.Services.AddSingleton<MachineState>(new MachineState(0, new List<Item>() { new Item() { Id = 1, Quantity = 5 }, new Item() { Id = 2, Quantity = 5 }, new Item() { Id = 3, Quantity = 5 } }));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
