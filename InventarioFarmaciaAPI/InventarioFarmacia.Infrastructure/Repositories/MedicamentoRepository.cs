using InventarioFarmacia.Domain.Entities;
using InventarioFarmacia.Infrastructure.Context;
using InventarioFarmacia.Infrastructure.Core;
using InventarioFarmacia.Infrastructure.Exceptions;
using InventarioFarmacia.Infrastructure.Interfaces;
using InventarioFarmacia.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmacia.Infrastructure.Repositories
{
    public class MedicamentoRepository : BaseRepository<Medicamento>, IMedicamentoRepository
    {
        public MedicamentoRepository(FarmaciaContext context) : base(context)
        {
        }

        public async Task<List<MedicamentoModel>> ListarAsync()
        {
            return await _dbSet
                .Select(m => new MedicamentoModel
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Descripcion = m.Descripcion,
                    Precio = m.Precio,
                    LaboratorioId = m.LaboratorioId
                })
                .ToListAsync();
        }

        public async Task<MedicamentoModel?> ObtenerAsync(int id)
        {
            return await _dbSet
                .Where(m => m.Id == id)
                .Select(m => new MedicamentoModel
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Descripcion = m.Descripcion,
                    Precio = m.Precio,
                    LaboratorioId = m.LaboratorioId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<MedicamentoModel> CrearAsync(MedicamentoModel model)
        {
            await Validar(model);

            var entidad = new Medicamento
            {
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                Precio = model.Precio,
                LaboratorioId = model.LaboratorioId
            };

            _dbSet.Add(entidad);
            await _context.SaveChangesAsync();

            model.Id = entidad.Id;
            return model;
        }

        public async Task ActualizarAsync(int id, MedicamentoModel model)
        {
            var entidad = await _dbSet.FindAsync(id)
                ?? throw new MedicamentoException("El medicamento no existe.");

            await Validar(model);

            entidad.Nombre = model.Nombre;
            entidad.Descripcion = model.Descripcion;
            entidad.Precio = model.Precio;
            entidad.LaboratorioId = model.LaboratorioId;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var entidad = await _dbSet.FindAsync(id)
                ?? throw new MedicamentoException("El medicamento no existe.");

            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }

        private async Task Validar(MedicamentoModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nombre))
                throw new MedicamentoException("El nombre del medicamento es obligatorio.");

            if (model.Precio < 0)
                throw new MedicamentoException("El precio no puede ser negativo.");

            var existeLab = await _context.Laboratorios
                .AnyAsync(l => l.Id == model.LaboratorioId);

            if (!existeLab)
                throw new MedicamentoException("El laboratorio indicado no existe.");
        }
    }
}