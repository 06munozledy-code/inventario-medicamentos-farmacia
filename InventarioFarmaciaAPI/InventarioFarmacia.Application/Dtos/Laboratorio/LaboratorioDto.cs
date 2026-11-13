namespace InventarioFarmacia.Application.Dtos.Laboratorio
{
    public class LaboratorioDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}