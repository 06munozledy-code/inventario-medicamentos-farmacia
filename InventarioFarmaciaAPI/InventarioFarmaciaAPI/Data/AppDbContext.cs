using InventarioFarmaciaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmaciaAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Laboratorio> Laboratorios { get; set; }
    public DbSet<Medicamento> Medicamentos { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<MovimientoInventario> Movimientos { get; set; }
}