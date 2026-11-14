using InventarioFarmacia.Infrastructure.Exceptions;
using InventarioFarmacia.Infrastructure.Interfaces;
using InventarioFarmacia.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventarioFarmaciaAPI.Controllers
{
    [ApiController]
    [Route("api/infra/medicamentos")]
    public class MedicamentosInfraController : ControllerBase
    {
        private readonly IMedicamentoRepository _repo;

        public MedicamentosInfraController(IMedicamentoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<MedicamentoModel>>> Listar()
        {
            return Ok(await _repo.ListarAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicamentoModel>> Obtener(int id)
        {
            var item = await _repo.ObtenerAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<MedicamentoModel>> Crear(MedicamentoModel model)
        {
            try
            {
                var creado = await _repo.CrearAsync(model);
                return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
            }
            catch (MedicamentoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, MedicamentoModel model)
        {
            try
            {
                await _repo.ActualizarAsync(id, model);
                return NoContent();
            }
            catch (MedicamentoException ex)
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
            catch (MedicamentoException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}