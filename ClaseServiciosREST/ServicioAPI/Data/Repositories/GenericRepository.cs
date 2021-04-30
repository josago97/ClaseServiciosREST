using Microsoft.EntityFrameworkCore;
using ServicioAPI.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ServicioAPI.Data.Repositories
{
    public class GenericRepository<T> where T : class, IEntity
    {
        protected readonly DataBase context;

        public GenericRepository(DataBase context)
        {
            this.context = context;
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>().AsNoTracking();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            await SaveAllAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                await SaveAllAsync();
            }
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await context.Set<T>().AnyAsync(e => e.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
