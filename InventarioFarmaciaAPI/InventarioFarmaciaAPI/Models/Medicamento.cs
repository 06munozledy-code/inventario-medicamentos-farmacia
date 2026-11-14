namespace InventarioFarmaciaAPI.Models;

public class Medicamento
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Concentracion { get; set; }
    public string? FormaFarmaceutica { get; set; }
    public int StockMinimo { get; set; }
    public bool Activo { get; set; } = true;

    public int LaboratorioId { get; set; }
    public Laboratorio? Laboratorio { get; set; }
}