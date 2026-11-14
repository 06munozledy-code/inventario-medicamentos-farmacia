using InventarioFarmacia.Infrastructure.Models;

namespace InventarioFarmacia.Infrastructure.Interfaces
{
    public interface IMedicamentoRepository
    {
        Task<List<MedicamentoModel>> ListarAsync();
        Task<MedicamentoModel?> ObtenerAsync(int id);
        Task<MedicamentoModel> CrearAsync(MedicamentoModel model);
        Task ActualizarAsync(int id, MedicamentoModel model);
        Task EliminarAsync(int id);
    }
}
