namespace InventarioFarmacia.Infrastructure.Models
{
    public class LoteModel
    {
        public int Id { get; set; }
        public string NumeroLote { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public int Cantidad { get; set; }
        public int MedicamentoId { get; set; }
    }
}