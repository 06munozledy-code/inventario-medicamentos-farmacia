using InventarioFarmacia.Domain.Entities;
using InventarioFarmacia.Infrastructure.Context;
using InventarioFarmacia.Infrastructure.Core;
using InventarioFarmacia.Infrastructure.Exceptions;
using InventarioFarmacia.Infrastructure.Interfaces;
using InventarioFarmacia.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmacia.Infrastructure.Repositories
{
    public class MovimientoInventarioRepository : BaseRepository<MovimientoInventario>, IMovimientoInventarioRepository
    {
        public MovimientoInventarioRepository(FarmaciaContext context) : base(context)
        {
        }

        public async Task<List<MovimientoInventarioModel>> ListarAsync()
        {
            return await _dbSet
                .Select(m => new MovimientoInventarioModel
                {
                    Id = m.Id,
                    TipoMovimiento = m.TipoMovimiento,
                    Cantidad = m.Cantidad,
                    Fecha = m.Fecha,
                    Observacion = m.Observacion,
                    LoteId = m.LoteId
                })
                .ToListAsync();
        }

        public async Task<MovimientoInventarioModel?> ObtenerAsync(int id)
        {
            return await _dbSet
                .Where(m => m.Id == id)
                .Select(m => new MovimientoInventarioModel
                {
                    Id = m.Id,
                    TipoMovimiento = m.TipoMovimiento,
                    Cantidad = m.Cantidad,
                    Fecha = m.Fecha,
                    Observacion = m.Observacion,
                    LoteId = m.LoteId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<MovimientoInventarioModel> CrearAsync(MovimientoInventarioModel model)
        {
            if (model.TipoMovimiento != "Entrada" && model.TipoMovimiento != "Salida")
                throw new MovimientoInventarioException("El tipo de movimiento debe ser Entrada o Salida.");

            if (model.Cantidad <= 0)
                throw new MovimientoInventarioException("La cantidad debe ser mayor que cero.");

            var lote = await _context.Lotes.FindAsync(model.LoteId)
                ?? throw new MovimientoInventarioException("El lote indicado no existe.");

            if (model.TipoMovimiento == "Salida" && lote.Cantidad < model.Cantidad)
                throw new MovimientoInventarioException("Stock insuficiente en el lote.");

            lote.Cantidad += model.TipoMovimiento == "Entrada" ? model.Cantidad : -model.Cantidad;

            var entidad = new MovimientoInventario
            {
                TipoMovimiento = model.TipoMovimiento,
                Cantidad = model.Cantidad,
                Observacion = model.Observacion,
                LoteId = model.LoteId
            };

            _dbSet.Add(entidad);
            await _context.SaveChangesAsync();

            model.Id = entidad.Id;
            model.Fecha = entidad.Fecha;
            return model;
        }
    }
}