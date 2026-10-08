using InventarioFarmaciaAPI.Data;
using InventarioFarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmaciaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MedicamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedicamentosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/medicamentos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Medicamento>>> GetMedicamentos()
    {
        return await _context.Medicamentos.ToListAsync();
    }

    // GET: api/medicamentos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Medicamento>> GetMedicamento(int id)
    {
        var medicamento = await _context.Medicamentos.FindAsync(id);

        if (medicamento == null)
        {
            return NotFound();
        }

        return medicamento;
    }

    // POST: api/medicamentos
    [HttpPost]
    public async Task<ActionResult<Medicamento>> PostMedicamento(Medicamento medicamento)
    {
        var existeLaboratorio = await _context.Laboratorios
            .AnyAsync(l => l.Id == medicamento.LaboratorioId);

        if (!existeLaboratorio)
        {
            return BadRequest("El laboratorio indicado no existe.");
        }

        _context.Medicamentos.Add(medicamento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMedicamento), new { id = medicamento.Id }, medicamento);
    }

    // PUT: api/medicamentos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMedicamento(int id, Medicamento medicamento)
    {
        if (id != medicamento.Id)
        {
            return BadRequest();
        }

        _context.Entry(medicamento).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Medicamentos.AnyAsync(m => m.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/medicamentos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedicamento(int id)
    {
        var medicamento = await _context.Medicamentos.FindAsync(id);

        if (medicamento == null)
        {
            return NotFound();
        }

        _context.Medicamentos.Remove(medicamento);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
