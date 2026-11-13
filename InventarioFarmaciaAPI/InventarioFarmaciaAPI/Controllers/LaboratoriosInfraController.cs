using InventarioFarmacia.Infrastructure.Exceptions;
using InventarioFarmacia.Infrastructure.Interfaces;
using InventarioFarmacia.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventarioFarmaciaAPI.Controllers
{
    [ApiController]
    [Route("api/infra/laboratorios")]
    public class LaboratoriosInfraController : ControllerBase
    {
        private readonly ILaboratorioRepository _repo;

        public LaboratoriosInfraController(ILaboratorioRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<LaboratorioModel>>> Listar()
        {
            return Ok(await _repo.ListarAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LaboratorioModel>> Obtener(int id)
        {
            var item = await _repo.ObtenerAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<LaboratorioModel>> Crear(LaboratorioModel model)
        {
            try
            {
                var creado = await _repo.CrearAsync(model);
                return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
            }
            catch (LaboratorioException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, LaboratorioModel model)
        {
            try
            {
                await _repo.ActualizarAsync(id, model);
                return NoContent();
            }
            catch (LaboratorioException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _repo.EliminarAsync(id);
                return NoContent();
            }
            catch (LaboratorioException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}