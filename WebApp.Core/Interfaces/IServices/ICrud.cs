namespace WebApp.Core.Interfaces.IServices
{
    public interface ICrud<TModel, in TCreateModel>
        where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync(params object[] keys);

        Task AddAsync(TCreateModel createModel);

        Task UpdateAsync(TModel model);

        Task DeleteAsync(int modelId);
    }
}
