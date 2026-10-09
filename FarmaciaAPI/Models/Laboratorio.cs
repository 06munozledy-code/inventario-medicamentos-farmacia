using System.ComponentModel.DataAnnotations;

namespace FarmaciaAPI.Models;

public class Laboratorio
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefono { get; set; }

    public List<Medicamento> Medicamentos { get; set; } = new();
}