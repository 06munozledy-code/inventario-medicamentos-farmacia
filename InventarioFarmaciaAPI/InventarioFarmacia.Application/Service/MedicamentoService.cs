using InventarioFarmacia.Application.Contract;
using InventarioFarmacia.Application.Core;
using InventarioFarmacia.Application.Dtos.Medicamento;
using InventarioFarmacia.Infrastructure.Exceptions;
using InventarioFarmacia.Infrastructure.Interfaces;
using InventarioFarmacia.Infrastructure.Models;

namespace InventarioFarmacia.Application.Service
{
    public class MedicamentoService : BaseService, IMedicamentoService
    {
        private readonly IMedicamentoRepository _repo;
        private readonly ILaboratorioRepository _laboratorioRepo;

        public MedicamentoService(IMedicamentoRepository repo, ILaboratorioRepository laboratorioRepo)
        {
            _repo = repo;
            _laboratorioRepo = laboratorioRepo;
        }

        public async Task<ServiceResult> GetAll()
        {
            var lista = await _repo.ListarAsync();
            var dtos = lista.Select(ToDto).ToList();
            return Ok(dtos, "Medicamentos obtenidos.");
        }

        public async Task<ServiceResult> GetById(int id)
        {
            var model = await _repo.ObtenerAsync(id);
            if (model is null)
                return Fail("El medicamento no existe.");

            return Ok(ToDto(model));
        }

        public async Task<ServiceResult> Add(MedicamentoDto dto)
        {
            var errores = await Validar(dto);
            if (errores.Count > 0)
                return Fail("Hay errores de validación.", errores);

            try
            {
                var creado = await _repo.CrearAsync(ToModel(dto));
                return Ok(ToDto(creado), "Medicamento creado correctamente.");
            }
            catch (MedicamentoException ex)
            {
                return Fail(ex.Message);
            }
        }

        public async Task<ServiceResult> Update(int id, MedicamentoDto dto)
        {
            var errores = await Validar(dto);
            if (errores.Count > 0)
                return Fail("Hay errores de validación.", errores);

            try
            {
                await _repo.ActualizarAsync(id, ToModel(dto));
                return Ok(null, "Medicamento actualizado correctamente.");
            }
            catch (MedicamentoException ex)
            {
                return Fail(ex.Message);
            }
        }

        public async Task<ServiceResult> Remove(int id)
        {
            try
            {
                await _repo.EliminarAsync(id);
                return Ok(null, "Medicamento eliminado correctamente.");
            }
            catch (MedicamentoException ex)
            {
                return Fail(ex.Message);
            }
        }

        private async Task<List<string>> Validar(MedicamentoDto dto)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errores.Add("El nombre es obligatorio.");
            else if (dto.Nombre.Trim().Length < 2 || dto.Nombre.Trim().Length > 100)
                errores.Add("El nombre debe tener entre 2 y 100 caracteres.");

            if (!string.IsNullOrWhiteSpace(dto.Descripcion) && dto.Descripcion.Trim().Length > 250)
                errores.Add("La descripción no puede tener más de 250 caracteres.");

            if (dto.Precio <= 0)
                errores.Add("El precio debe ser mayor que cero.");
            else if (dto.Precio > 1000000)
                errores.Add("El precio no puede ser mayor que 1,000,000.");

            if (dto.LaboratorioId <= 0)
                errores.Add("Debe indicar un laboratorio válido.");
            else if (await _laboratorioRepo.ObtenerAsync(dto.LaboratorioId) is null)
                errores.Add("El laboratorio indicado no existe.");

            return errores;
        }

        private static MedicamentoDto ToDto(MedicamentoModel m) => new()
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Descripcion = m.Descripcion,
            Precio = m.Precio,
            LaboratorioId = m.LaboratorioId
        };

        private static MedicamentoModel ToModel(MedicamentoDto d) => new()
        {
            Id = d.Id,
            Nombre = d.Nombre.Trim(),
            Descripcion = d.Descripcion?.Trim(),
            Precio = d.Precio,
            LaboratorioId = d.LaboratorioId
        };
    }
}