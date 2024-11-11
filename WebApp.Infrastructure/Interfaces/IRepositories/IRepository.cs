namespace WebApp.Infrastructure.Interfaces.IRepositories;

using WebApp.Infrastructure.Entities;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(params object[] keys);

    Task<object> AddAsync(TEntity entity);

    void Delete(TEntity entity);

    Task DeleteByIdAsync(params object[] keys);

    void Update(TEntity entity);

    bool IsExist(params object[] keys);
}
