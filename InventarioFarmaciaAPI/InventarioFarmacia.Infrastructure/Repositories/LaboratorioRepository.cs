using InventarioFarmacia.Domain.Entities;
using InventarioFarmacia.Infrastructure.Context;
using InventarioFarmacia.Infrastructure.Core;
using InventarioFarmacia.Infrastructure.Exceptions;
using InventarioFarmacia.Infrastructure.Interfaces;
using InventarioFarmacia.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmacia.Infrastructure.Repositories
{
    public class LaboratorioRepository : BaseRepository<Laboratorio>, ILaboratorioRepository
    {
        public LaboratorioRepository(FarmaciaContext context) : base(context)
        {
        }

        public async Task<List<LaboratorioModel>> ListarAsync()
        {
            return await _dbSet
                .Select(l => new LaboratorioModel
                {
                    Id = l.Id,
                    Nombre = l.Nombre,
                    Telefono = l.Telefono
                })
                .ToListAsync();
        }

        public async Task<LaboratorioModel?> ObtenerAsync(int id)
        {
            return await _dbSet
                .Where(l => l.Id == id)
                .Select(l => new LaboratorioModel
                {
                    Id = l.Id,
                    Nombre = l.Nombre,
                    Telefono = l.Telefono
                })
                .FirstOrDefaultAsync();
        }

        public async Task<LaboratorioModel> CrearAsync(LaboratorioModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nombre))
                throw new LaboratorioException("El nombre del laboratorio es obligatorio.");

            var entidad = new Laboratorio
            {
                Nombre = model.Nombre,
                Telefono = model.Telefono
            };

            _dbSet.Add(entidad);
            await _context.SaveChangesAsync();

            model.Id = entidad.Id;
            return model;
        }

        public async Task ActualizarAsync(int id, LaboratorioModel model)
        {
            var entidad = await _dbSet.FindAsync(id)
                ?? throw new LaboratorioException("El laboratorio no existe.");

            if (string.IsNullOrWhiteSpace(model.Nombre))
                throw new LaboratorioException("El nombre del laboratorio es obligatorio.");

            entidad.Nombre = model.Nombre;
            entidad.Telefono = model.Telefono;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var entidad = await _dbSet.FindAsync(id)
                ?? throw new LaboratorioException("El laboratorio no existe.");

            var tieneMedicamentos = await _context.Medicamentos
                .AnyAsync(m => m.LaboratorioId == id);

            if (tieneMedicamentos)
                throw new LaboratorioException("No se puede eliminar un laboratorio que tiene medicamentos.");

            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }
    }
}