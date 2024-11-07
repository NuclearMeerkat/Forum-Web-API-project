namespace WebApp.Core.Interfaces.IServices
{
    public interface ICrud<TModel, in TCreateModel, in TUpdateModel, in TQueryParametersModel, TKey>
        where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync(TQueryParametersModel queryParameters);

        Task<TModel> GetByIdAsync(params object[] keys);

        Task<TKey> AddAsync(TCreateModel model);

        Task UpdateAsync(TUpdateModel model);

        Task DeleteAsync(int modelId);
    }
}
