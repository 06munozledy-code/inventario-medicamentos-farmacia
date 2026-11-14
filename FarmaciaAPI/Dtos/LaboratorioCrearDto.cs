using System.ComponentModel.DataAnnotations;

namespace FarmaciaAPI.Dtos;

public class LaboratorioCrearDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefono { get; set; }
}