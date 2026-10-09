namespace InventarioFarmaciaAPI.Models;

public class MovimientoInventario
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty; // "Entrada" o "Salida"
    public int Cantidad { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string? Motivo { get; set; }

    public int LoteId { get; set; }
    public Lote? Lote { get; set; }
}