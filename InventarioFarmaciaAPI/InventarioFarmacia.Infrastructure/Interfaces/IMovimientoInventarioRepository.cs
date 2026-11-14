using InventarioFarmacia.Infrastructure.Models;

namespace InventarioFarmacia.Infrastructure.Interfaces
{
    public interface IMovimientoInventarioRepository
    {
        Task<List<MovimientoInventarioModel>> ListarAsync();
        Task<MovimientoInventarioModel?> ObtenerAsync(int id);
        Task<MovimientoInventarioModel> CrearAsync(MovimientoInventarioModel model);
    }
}