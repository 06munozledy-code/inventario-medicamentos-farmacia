namespace InventarioFarmaciaAPI.Models;

public class Laboratorio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public bool Activo { get; set; } = true;

    public List<Medicamento> Medicamentos { get; set; } = new();
}
