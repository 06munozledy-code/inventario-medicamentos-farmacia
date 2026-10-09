using System.ComponentModel.DataAnnotations;

namespace FarmaciaAPI.Dtos;

public class MedicamentoCrearDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Descripcion { get; set; }

    [Range(0, 100000)]
    public decimal Precio { get; set; }

    [Range(0, 1000000)]
    public int Stock { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public int LaboratorioId { get; set; }
}