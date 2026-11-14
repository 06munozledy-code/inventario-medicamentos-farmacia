using InventarioFarmaciaAPI.Data;
using InventarioFarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmaciaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LotesController : ControllerBase
{
    private readonly AppDbContext _context;

    public LotesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/lotes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lote>>> GetLotes()
    {
        return await _context.Lotes
            .OrderBy(l => l.FechaVencimiento)
            .ToListAsync();
    }

    // GET: api/lotes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Lote>> GetLote(int id)
    {
        var lote = await _context.Lotes.FindAsync(id);

        if (lote == null)
        {
            return NotFound();
        }

        return lote;
    }

    // POST: api/lotes
    [HttpPost]
    public async Task<ActionResult<Lote>> PostLote(Lote lote)
    {
        var existeMedicamento = await _context.Medicamentos
            .AnyAsync(m => m.Id == lote.MedicamentoId);

        if (!existeMedicamento)
        {
            return BadRequest("El medicamento indicado no existe.");
        }

        if (lote.FechaVencimiento <= lote.FechaFabricacion)
        {
            return BadRequest("La fecha de vencimiento debe ser posterior a la de fabricación.");
        }

        var numeroRepetido = await _context.Lotes.AnyAsync(l =>
            l.MedicamentoId == lote.MedicamentoId && l.NumeroLote == lote.NumeroLote);

        if (numeroRepetido)
        {
            return BadRequest("Ya existe un lote con ese número para este medicamento.");
        }

        _context.Lotes.Add(lote);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLote), new { id = lote.Id }, lote);
    }

    // PUT: api/lotes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLote(int id, Lote lote)
    {
        if (id != lote.Id)
        {
            return BadRequest();
        }

        _context.Entry(lote).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Lotes.AnyAsync(l => l.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/lotes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLote(int id)
    {
        var lote = await _context.Lotes.FindAsync(id);

        if (lote == null)
        {
            return NotFound();
        }

        _context.Lotes.Remove(lote);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}