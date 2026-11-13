using InventarioFarmacia.Application.Dtos;

namespace InventarioFarmacia.Application.Core
{
    public interface IBaseService<TDto> where TDto : DtoBase
    {
        Task<ServiceResult> GetAll();
        Task<ServiceResult> GetById(int id);
        Task<ServiceResult> Add(TDto dto);
        Task<ServiceResult> Update(int id, TDto dto);
        Task<ServiceResult> Remove(int id);
    }
}