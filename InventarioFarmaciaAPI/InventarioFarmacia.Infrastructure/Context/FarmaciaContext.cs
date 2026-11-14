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
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<MovimientoInventario> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicamento>()
                .Property(m => m.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Lote>()
                .HasOne(l => l.Medicamento)
                .WithMany()
                .HasForeignKey(l => l.MedicamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimientoInventario>()
                .HasOne(m => m.Lote)
                .WithMany(l => l.Movimientos)
                .HasForeignKey(m => m.LoteId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}