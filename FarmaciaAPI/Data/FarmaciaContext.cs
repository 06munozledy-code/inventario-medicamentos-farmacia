using FarmaciaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FarmaciaAPI.Data;

public class FarmaciaContext : DbContext
{
    public FarmaciaContext(
        DbContextOptions<FarmaciaContext> options) : base(options)
    {
    }

    public DbSet<Laboratorio> Laboratorios { get; set; }
    public DbSet<Medicamento> Medicamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Laboratorio>()
            .HasIndex(l => l.Nombre)
            .IsUnique();

        modelBuilder.Entity<Medicamento>()
            .HasIndex(m => m.Nombre)
            .IsUnique();
    }
}
