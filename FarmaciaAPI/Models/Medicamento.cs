using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaciaAPI.Models;

public class Medicamento
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public int LaboratorioId { get; set; }
    public Laboratorio? Laboratorio { get; set; }
}