using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.DataAccess.Data;

namespace WebApp.DataAccess.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        public ForumDbContext context { get; }

        protected GenericRepository(ForumDbContext context)
        {
            this.context = context;
        }

        public async Task<object> AddAsync(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
            await this.context.SaveChangesAsync();

            return entity.GetIdentifier();
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

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this.context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(params object[] keys)
        {
            return await this.context.Set<T>().FindAsync(keys);
        }

        public void Update(T entity)
        {
            this.context.Set<T>().Update(entity);
            this.context.SaveChanges();
        }

        public bool IsExist(params object[] keys)
        {
            return this.context.Set<T>().Find(keys) != null;
        }
    }
}
