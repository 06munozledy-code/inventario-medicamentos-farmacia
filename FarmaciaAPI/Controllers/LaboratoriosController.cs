using FarmaciaAPI.Data;
using FarmaciaAPI.Dtos;
using FarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmaciaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LaboratoriosController : ControllerBase
{
    private readonly FarmaciaContext _context;

    public LaboratoriosController(FarmaciaContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<LaboratorioDto>> CrearLaboratorio(
        LaboratorioCrearDto dto)
    {
        if (await _context.Laboratorios.AnyAsync(l => l.Nombre == dto.Nombre))
            return BadRequest("Ya existe un laboratorio con ese nombre.");

        var laboratorio = new Laboratorio
        {
            Nombre = dto.Nombre,
            Telefono = dto.Telefono
        };

        _context.Laboratorios.Add(laboratorio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(VerLaboratorio),
            new { id = laboratorio.Id },
            ConvertirADto(laboratorio));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LaboratorioDto>>> ListaLaboratorios()
    {
        var lista = await _context.Laboratorios.ToListAsync();
        return lista.Select(ConvertirADto).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LaboratorioDto>> VerLaboratorio(int id)
    {
        var laboratorio = await _context.Laboratorios.FindAsync(id);

        if (laboratorio == null)
            return NotFound();

        return ConvertirADto(laboratorio);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        int id, LaboratorioCrearDto dto)
    {
        var laboratorio = await _context.Laboratorios.FindAsync(id);

        if (laboratorio == null)
            return NotFound();

        if (await _context.Laboratorios.AnyAsync(
            l => l.Nombre == dto.Nombre && l.Id != id))
            return BadRequest("Ya existe un laboratorio con ese nombre.");

        laboratorio.Nombre = dto.Nombre;
        laboratorio.Telefono = dto.Telefono;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarLaboratorio(int id)
    {
        var laboratorio = await _context.Laboratorios.FindAsync(id);

        if (laboratorio == null)
            return NotFound();

        if (await _context.Medicamentos.AnyAsync(
            m => m.LaboratorioId == id))
            return BadRequest(
                "No se puede eliminar: el laboratorio tiene medicamentos.");

        _context.Laboratorios.Remove(laboratorio);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static LaboratorioDto ConvertirADto(Laboratorio laboratorio)
    {
        return new LaboratorioDto
        {
            Id = laboratorio.Id,
            Nombre = laboratorio.Nombre,
            Telefono = laboratorio.Telefono
        };
    }
}