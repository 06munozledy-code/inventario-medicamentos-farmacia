using InventarioFarmacia.Domain.Core;

namespace InventarioFarmacia.Domain.Entities
{
    public class Laboratorio : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}