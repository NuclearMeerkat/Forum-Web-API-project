namespace WebApp.Infrastructure.Interfaces.IServices
{
    public interface ICrud<TModel, in TCreateModel, in TUpdateModel, in TQueryParametersModel, TKey>
        where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync(TQueryParametersModel? queryParameters = default);

        Task<TModel> GetByIdAsync(params object[] keys);

        Task<TKey> RegisterAsync(TCreateModel model);

        Task UpdateAsync(TUpdateModel model);

        Task DeleteAsync(TKey modelId);
    }
}
