using InventarioFarmacia.Application.Contract;
using InventarioFarmacia.Application.Dtos.Medicamento;
using Microsoft.AspNetCore.Mvc;

namespace InventarioFarmaciaAPI.Controllers
{
    [ApiController]
    [Route("api/servicios/medicamentos")]
    public class MedicamentosServiceController : ControllerBase
    {
        private readonly IMedicamentoService _service;

        public MedicamentosServiceController(IMedicamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MedicamentoDto dto)
        {
            var result = await _service.Add(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MedicamentoDto dto)
        {
            var result = await _service.Update(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _service.Remove(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}