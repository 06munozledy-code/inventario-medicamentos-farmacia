using InventarioFarmaciaAPI.Data;
using InventarioFarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmaciaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LaboratoriosController : ControllerBase
{
    private readonly AppDbContext _context;

    public LaboratoriosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/laboratorios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Laboratorio>>> GetLaboratorios()
    {
        return await _context.Laboratorios.ToListAsync();
    }

    // GET: api/laboratorios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Laboratorio>> GetLaboratorio(int id)
    {
        var laboratorio = await _context.Laboratorios.FindAsync(id);

        if (laboratorio == null)
        {
            return NotFound();
        }

        return laboratorio;
    }

    // POST: api/laboratorios
    [HttpPost]
    public async Task<ActionResult<Laboratorio>> PostLaboratorio(Laboratorio laboratorio)
    {
        _context.Laboratorios.Add(laboratorio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLaboratorio), new { id = laboratorio.Id }, laboratorio);
    }

    // PUT: api/laboratorios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLaboratorio(int id, Laboratorio laboratorio)
    {
        if (id != laboratorio.Id)
        {
            return BadRequest();
        }

        _context.Entry(laboratorio).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Laboratorios.AnyAsync(l => l.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/laboratorios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLaboratorio(int id)
    {
        var laboratorio = await _context.Laboratorios.FindAsync(id);

        if (laboratorio == null)
        {
            return NotFound();
        }

        _context.Laboratorios.Remove(laboratorio);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
