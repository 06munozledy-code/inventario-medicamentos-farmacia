using InventarioFarmacia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmacia.Infrastructure.Context
{
    public class FarmaciaContext : DbContext
    {
        public FarmaciaContext(DbContextOptions<FarmaciaContext> options) : base(options)
        {
        }

        public DbSet<Laboratorio> Laboratorios { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicamento>()
                .Property(m => m.Precio)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}