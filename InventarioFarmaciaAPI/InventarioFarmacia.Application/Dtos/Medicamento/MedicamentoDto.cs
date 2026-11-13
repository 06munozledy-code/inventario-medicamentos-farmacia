namespace InventarioFarmacia.Application.Dtos.Medicamento
{
    public class MedicamentoDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int LaboratorioId { get; set; }
    }
}