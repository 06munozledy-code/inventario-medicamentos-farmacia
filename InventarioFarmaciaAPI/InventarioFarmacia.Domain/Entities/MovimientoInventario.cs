using InventarioFarmacia.Domain.Core;

namespace InventarioFarmacia.Domain.Entities
{
    public class MovimientoInventario : BaseEntity
    {
        public string TipoMovimiento { get; set; } = string.Empty; // "Entrada" o "Salida"
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Observacion { get; set; }

        public int LoteId { get; set; }
        public Lote? Lote { get; set; }
    }
}