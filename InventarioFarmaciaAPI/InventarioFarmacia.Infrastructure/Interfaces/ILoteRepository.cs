using InventarioFarmacia.Infrastructure.Models;

namespace InventarioFarmacia.Infrastructure.Interfaces
{
    public interface ILoteRepository
    {
        Task<List<LoteModel>> ListarAsync();
        Task<LoteModel?> ObtenerAsync(int id);
        Task<LoteModel> CrearAsync(LoteModel model);
        Task ActualizarAsync(int id, LoteModel model);
        Task EliminarAsync(int id);
    }
}