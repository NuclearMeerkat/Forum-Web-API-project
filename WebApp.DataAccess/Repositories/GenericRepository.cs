using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Entities;
using WebApp.DataAccess.Interfaces;

namespace WebApp.DataAccess.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private ForumDbContext context { get; set; }

        protected GenericRepository(ForumDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            this.context.Set<T>().Remove(entity);
            this.context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(params object[] keys)
        {
            var entity = await this.context.Set<T>().FindAsync(keys);
            if (entity != null)
            {
                this.context.Set<T>().Remove(entity);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this.context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(params object[] keys)
        {
            return await this.context.Set<T>().FindAsync(keys);
        }

        public void Update(T entity)
        {
            this.context.Set<T>().Update(entity);
            this.context.SaveChanges();
        }
    }
}
