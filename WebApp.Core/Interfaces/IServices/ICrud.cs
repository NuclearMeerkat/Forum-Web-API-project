namespace WebApp.Core.Interfaces.IServices
{
    public interface ICrud<TModel, in TCreateModel, TKey>
        where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync(params object[] keys);

        Task<TKey> AddAsync(TCreateModel model);

        Task UpdateAsync(TCreateModel model);

        Task DeleteAsync(int modelId);
    }
}
