using FarmaciaAPI.Data;
using FarmaciaAPI.Dtos;
using FarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmaciaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MedicamentosController : ControllerBase
{
    private readonly FarmaciaContext _context;

    public MedicamentosController(FarmaciaContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<MedicamentoDto>> CrearMedicamento(
        MedicamentoCrearDto dto)
    {
        var laboratorio = await _context.Laboratorios
            .FindAsync(dto.LaboratorioId);

        if (laboratorio == null)
            return BadRequest("El laboratorio indicado no existe.");

        if (await _context.Medicamentos.AnyAsync(m => m.Nombre == dto.Nombre))
            return BadRequest("Ya existe un medicamento con ese nombre.");

        var medicamento = new Medicamento
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock,
            FechaVencimiento = dto.FechaVencimiento,
            LaboratorioId = dto.LaboratorioId,
            Laboratorio = laboratorio
        };

        _context.Medicamentos.Add(medicamento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(VerMedicamento),
            new { id = medicamento.Id },
            ConvertirADto(medicamento));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicamentoDto>>> ListaMedicamentos()
    {
        var lista = await _context.Medicamentos
            .Include(m => m.Laboratorio)
            .ToListAsync();

        return lista.Select(ConvertirADto).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicamentoDto>> VerMedicamento(int id)
    {
        var medicamento = await _context.Medicamentos
            .Include(m => m.Laboratorio)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medicamento == null)
            return NotFound();

        return ConvertirADto(medicamento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        int id, MedicamentoCrearDto dto)
    {
        var medicamento = await _context.Medicamentos.FindAsync(id);

        if (medicamento == null)
            return NotFound();

        if (!await _context.Laboratorios
            .AnyAsync(l => l.Id == dto.LaboratorioId))
            return BadRequest("El laboratorio indicado no existe.");

        if (await _context.Medicamentos.AnyAsync(
            m => m.Nombre == dto.Nombre && m.Id != id))
            return BadRequest("Ya existe un medicamento con ese nombre.");

        medicamento.Nombre = dto.Nombre;
        medicamento.Descripcion = dto.Descripcion;
        medicamento.Precio = dto.Precio;
        medicamento.Stock = dto.Stock;
        medicamento.FechaVencimiento = dto.FechaVencimiento;
        medicamento.LaboratorioId = dto.LaboratorioId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarMedicamento(int id)
    {
        var medicamento = await _context.Medicamentos.FindAsync(id);

        if (medicamento == null)
            return NotFound();

        _context.Medicamentos.Remove(medicamento);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static MedicamentoDto ConvertirADto(Medicamento medicamento)
    {
        return new MedicamentoDto
        {
            Id = medicamento.Id,
            Nombre = medicamento.Nombre,
            Descripcion = medicamento.Descripcion,
            Precio = medicamento.Precio,
            Stock = medicamento.Stock,
            FechaVencimiento = medicamento.FechaVencimiento,
            LaboratorioId = medicamento.LaboratorioId,
            LaboratorioNombre = medicamento.Laboratorio?.Nombre
        };
    }
}
