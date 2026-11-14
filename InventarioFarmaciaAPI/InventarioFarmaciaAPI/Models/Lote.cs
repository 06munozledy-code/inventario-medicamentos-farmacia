namespace InventarioFarmaciaAPI.Models;

public class Lote
{
    public int Id { get; set; }
    public string NumeroLote { get; set; } = string.Empty;
    public DateTime FechaFabricacion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int CantidadDisponible { get; set; }
    public bool Activo { get; set; } = true;

    public int MedicamentoId { get; set; }
    public Medicamento? Medicamento { get; set; }

    public List<MovimientoInventario> Movimientos { get; set; } = new();
}