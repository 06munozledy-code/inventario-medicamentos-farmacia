using InventarioFarmacia.Infrastructure.Models;

namespace InventarioFarmacia.Infrastructure.Interfaces
{
    public interface ILaboratorioRepository
    {
        Task<List<LaboratorioModel>> ListarAsync();
        Task<LaboratorioModel?> ObtenerAsync(int id);
        Task<LaboratorioModel> CrearAsync(LaboratorioModel model);
        Task ActualizarAsync(int id, LaboratorioModel model);
        Task EliminarAsync(int id);
    }
}
