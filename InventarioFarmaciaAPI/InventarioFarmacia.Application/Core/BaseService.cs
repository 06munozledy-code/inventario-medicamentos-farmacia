namespace InventarioFarmacia.Application.Core
{
    public abstract class BaseService
    {
        protected ServiceResult Ok(object? data = null, string message = "Operación exitosa.")
        {
            return new ServiceResult { Success = true, Message = message, Data = data };
        }

        protected ServiceResult Fail(string message, List<string>? errors = null)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}