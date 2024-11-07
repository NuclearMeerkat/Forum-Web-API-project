using WebApp.Core.Entities;

namespace WebApp.Core.Interfaces.IRepositories;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(params object[] keys);

    Task<object> AddAsync(TEntity entity);

    void Delete(TEntity entity);

    Task DeleteByIdAsync(params object[] keys);

    void Update(TEntity entity);
}
