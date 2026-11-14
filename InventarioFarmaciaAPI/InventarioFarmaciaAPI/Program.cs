using InventarioFarmaciaAPI.Data;
using InventarioFarmacia.Infrastructure.Context;
using InventarioFarmacia.Infrastructure.Interfaces;
using InventarioFarmacia.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Contexto de la Tarea 2 (se mantiene por ahora)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// NUEVO: contexto de la capa Infrastructure (base de datos aparte)
builder.Services.AddDbContext<FarmaciaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InfraConnection")));

// NUEVO: repositorios
builder.Services.AddScoped<ILaboratorioRepository, LaboratorioRepository>();
builder.Services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
builder.Services.AddScoped<ILoteRepository, LoteRepository>();
builder.Services.AddScoped<IMovimientoInventarioRepository, MovimientoInventarioRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();