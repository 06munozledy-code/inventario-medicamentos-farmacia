using System.Text.RegularExpressions;
using InventarioFarmacia.Application.Contract;
using InventarioFarmacia.Application.Core;
using InventarioFarmacia.Application.Dtos.Laboratorio;
using InventarioFarmacia.Infrastructure.Exceptions;
using InventarioFarmacia.Infrastructure.Interfaces;
using InventarioFarmacia.Infrastructure.Models;

namespace InventarioFarmacia.Application.Service
{
    public class LaboratorioService : BaseService, ILaboratorioService
    {
        private readonly ILaboratorioRepository _repo;

        public LaboratorioService(ILaboratorioRepository repo)
        {
            _repo = repo;
        }

        public async Task<ServiceResult> GetAll()
        {
            var lista = await _repo.ListarAsync();
            var dtos = lista.Select(ToDto).ToList();
            return Ok(dtos, "Laboratorios obtenidos.");
        }

        public async Task<ServiceResult> GetById(int id)
        {
            var model = await _repo.ObtenerAsync(id);
            if (model is null)
                return Fail("El laboratorio no existe.");

            return Ok(ToDto(model));
        }

        public async Task<ServiceResult> Add(LaboratorioDto dto)
        {
            var errores = Validar(dto);
            if (errores.Count > 0)
                return Fail("Hay errores de validación.", errores);

            try
            {
                var creado = await _repo.CrearAsync(ToModel(dto));
                return Ok(ToDto(creado), "Laboratorio creado correctamente.");
            }
            catch (LaboratorioException ex)
            {
                return Fail(ex.Message);
            }
        }

        public async Task<ServiceResult> Update(int id, LaboratorioDto dto)
        {
            var errores = Validar(dto);
            if (errores.Count > 0)
                return Fail("Hay errores de validación.", errores);

            try
            {
                await _repo.ActualizarAsync(id, ToModel(dto));
                return Ok(null, "Laboratorio actualizado correctamente.");
            }
            catch (LaboratorioException ex)
            {
                return Fail(ex.Message);
            }
        }

        public async Task<ServiceResult> Remove(int id)
        {
            try
            {
                await _repo.EliminarAsync(id);
                return Ok(null, "Laboratorio eliminado correctamente.");
            }
            catch (LaboratorioException ex)
            {
                return Fail(ex.Message);
            }
        }

        private static List<string> Validar(LaboratorioDto dto)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errores.Add("El nombre es obligatorio.");
            else if (dto.Nombre.Trim().Length < 2 || dto.Nombre.Trim().Length > 100)
                errores.Add("El nombre debe tener entre 2 y 100 caracteres.");

            if (!string.IsNullOrWhiteSpace(dto.Telefono) &&
                !Regex.IsMatch(dto.Telefono, @"^[0-9\-\+\(\)\s]{7,20}$"))
                errores.Add("El teléfono solo puede tener números, espacios y los símbolos + - ( ), de 7 a 20 caracteres.");

            return errores;
        }

        private static LaboratorioDto ToDto(LaboratorioModel m) => new()
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Telefono = m.Telefono
        };

        private static LaboratorioModel ToModel(LaboratorioDto d) => new()
        {
            Id = d.Id,
            Nombre = d.Nombre.Trim(),
            Telefono = d.Telefono?.Trim()
        };
    }
}