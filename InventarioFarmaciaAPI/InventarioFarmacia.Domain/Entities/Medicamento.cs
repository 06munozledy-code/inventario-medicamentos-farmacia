using InventarioFarmacia.Domain.Core;

namespace InventarioFarmacia.Domain.Entities
{
    public class Medicamento : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int LaboratorioId { get; set; }
        public Laboratorio? Laboratorio { get; set; }
    }
}