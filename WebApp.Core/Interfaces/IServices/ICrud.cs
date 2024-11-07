namespace WebApp.Core.Interfaces.IServices
{
    public interface ICrud<TModel, in TCreateModel>
        where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync(params object[] keys);

        Task AddAsync(TCreateModel model);

        Task UpdateAsync(TCreateModel model);

        Task DeleteAsync(int modelId);
    }
}
