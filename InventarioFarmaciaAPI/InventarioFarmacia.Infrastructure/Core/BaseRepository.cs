using InventarioFarmacia.Domain.Core;
using InventarioFarmacia.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace InventarioFarmacia.Infrastructure.Core
{
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        protected readonly FarmaciaContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(FarmaciaContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
    }
}