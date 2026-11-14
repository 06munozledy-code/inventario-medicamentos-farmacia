using InventarioFarmacia.Domain.Entities;
using InventarioFarmacia.Infrastructure.Context;
using InventarioFarmacia.Infrastructure.Core;
using InventarioFarmacia.Infrastructure.Exceptions;
using InventarioFarmacia.Infrastructure.Interfaces;
using InventarioFarmacia.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmacia.Infrastructure.Repositories
{
    public class LoteRepository : BaseRepository<Lote>, ILoteRepository
    {
        public LoteRepository(FarmaciaContext context) : base(context)
        {
        }

        public async Task<List<LoteModel>> ListarAsync()
        {
            return await _dbSet
                .Select(l => new LoteModel
                {
                    Id = l.Id,
                    NumeroLote = l.NumeroLote,
                    FechaVencimiento = l.FechaVencimiento,
                    Cantidad = l.Cantidad,
                    MedicamentoId = l.MedicamentoId
                })
                .ToListAsync();
        }

        public async Task<LoteModel?> ObtenerAsync(int id)
        {
            return await _dbSet
                .Where(l => l.Id == id)
                .Select(l => new LoteModel
                {
                    Id = l.Id,
                    NumeroLote = l.NumeroLote,
                    FechaVencimiento = l.FechaVencimiento,
                    Cantidad = l.Cantidad,
                    MedicamentoId = l.MedicamentoId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<LoteModel> CrearAsync(LoteModel model)
        {
            await Validar(model);

            var entidad = new Lote
            {
                NumeroLote = model.NumeroLote,
                FechaVencimiento = model.FechaVencimiento,
                Cantidad = model.Cantidad,
                MedicamentoId = model.MedicamentoId
            };

            _dbSet.Add(entidad);
            await _context.SaveChangesAsync();

            model.Id = entidad.Id;
            return model;
        }

        public async Task ActualizarAsync(int id, LoteModel model)
        {
            var entidad = await _dbSet.FindAsync(id)
                ?? throw new LoteException("El lote no existe.");

            await Validar(model);

            entidad.NumeroLote = model.NumeroLote;
            entidad.FechaVencimiento = model.FechaVencimiento;
            entidad.Cantidad = model.Cantidad;
            entidad.MedicamentoId = model.MedicamentoId;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var entidad = await _dbSet.FindAsync(id)
                ?? throw new LoteException("El lote no existe.");

            var tieneMovimientos = await _context.Movimientos.AnyAsync(m => m.LoteId == id);
            if (tieneMovimientos)
                throw new LoteException("No se puede eliminar un lote que tiene movimientos.");

            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }

        private async Task Validar(LoteModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NumeroLote))
                throw new LoteException("El número de lote es obligatorio.");

            if (model.Cantidad < 0)
                throw new LoteException("La cantidad no puede ser negativa.");

            var existeMed = await _context.Medicamentos
                .AnyAsync(m => m.Id == model.MedicamentoId);

            if (!existeMed)
                throw new LoteException("El medicamento indicado no existe.");
        }
    }
}