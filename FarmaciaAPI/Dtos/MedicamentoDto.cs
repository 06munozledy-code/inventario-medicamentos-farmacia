namespace FarmaciaAPI.Dtos;

public class MedicamentoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int LaboratorioId { get; set; }
    public string? LaboratorioNombre { get; set; }
}