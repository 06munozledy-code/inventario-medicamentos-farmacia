using InventarioFarmaciaAPI.Data;
using InventarioFarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmaciaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovimientosController : ControllerBase
{
    private readonly AppDbContext _context;

    public MovimientosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/movimientos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovimientoInventario>>> GetMovimientos()
    {
        return await _context.Movimientos
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }

    // GET: api/movimientos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovimientoInventario>> GetMovimiento(int id)
    {
        var movimiento = await _context.Movimientos.FindAsync(id);

        if (movimiento == null)
        {
            return NotFound();
        }

        return movimiento;
    }

    // POST: api/movimientos
    [HttpPost]
    public async Task<IActionResult> PostMovimiento(MovimientoInventario movimiento)
    {
        if (movimiento.Cantidad <= 0)
        {
            return BadRequest("La cantidad debe ser mayor que cero.");
        }

        var tipo = movimiento.Tipo?.Trim().ToLower();

        if (tipo != "entrada" && tipo != "salida")
        {
            return BadRequest("El tipo debe ser 'Entrada' o 'Salida'.");
        }

        var lote = await _context.Lotes.FindAsync(movimiento.LoteId);

        if (lote == null)
        {
            return BadRequest("El lote indicado no existe.");
        }

        if (tipo == "salida")
        {
            if (movimiento.Cantidad > lote.CantidadDisponible)
            {
                return BadRequest("La salida no puede superar la existencia del lote.");
            }

            lote.CantidadDisponible -= movimiento.Cantidad;
            movimiento.Tipo = "Salida";
        }
        else
        {
            lote.CantidadDisponible += movimiento.Cantidad;
            movimiento.Tipo = "Entrada";
        }

        _context.Movimientos.Add(movimiento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovimiento), new { id = movimiento.Id }, new
        {
            movimiento.Id,
            movimiento.Tipo,
            movimiento.Cantidad,
            movimiento.Fecha,
            movimiento.Motivo,
            movimiento.LoteId,
            ExistenciaActual = lote.CantidadDisponible
        });
    }

    // DELETE: api/movimientos/5 (revierte el efecto sobre la existencia del lote)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovimiento(int id)
    {
        var movimiento = await _context.Movimientos.FindAsync(id);

        if (movimiento == null)
        {
            return NotFound();
        }

        var lote = await _context.Lotes.FindAsync(movimiento.LoteId);

        if (lote != null)
        {
            if (movimiento.Tipo == "Entrada")
            {
                if (lote.CantidadDisponible < movimiento.Cantidad)
                {
                    return BadRequest("No se puede eliminar: la existencia del lote quedaría negativa.");
                }

                lote.CantidadDisponible -= movimiento.Cantidad;
            }
            else
            {
                lote.CantidadDisponible += movimiento.Cantidad;
            }
        }

        _context.Movimientos.Remove(movimiento);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}