using WebApp.DataAccess.Entities;

namespace WebApp.DataAccess.Interfaces;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(params object[] keys);

    Task AddAsync(TEntity entity);

    void Delete(TEntity entity);

    Task DeleteByIdAsync(params object[] keys);

    void Update(TEntity entity);
}
