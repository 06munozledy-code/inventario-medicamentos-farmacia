using InventarioFarmacia.Domain.Core;

namespace InventarioFarmacia.Domain.Entities
{
    public class Lote : BaseEntity
    {
        public string NumeroLote { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public int Cantidad { get; set; }

        public int MedicamentoId { get; set; }
        public Medicamento? Medicamento { get; set; }

        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
    }
}