namespace WebApp.Infrastructure.Interfaces.IServices;

using WebApp.Infrastructure.Models.TopicModels;

public interface ITopicService
{
    Task<IEnumerable<TopicSummaryModel>> GetAllAsync(TopicQueryParametersModel? queryParameters = default);

    Task<TopicSummaryModel> GetByIdAsync(params object[] keys);

    Task<int> AddAsync(AdminTopicCreateModel model);

    public Task UpdateAsync(TopicUpdateModel model, int? ownerUserId = null);

    public Task DeleteAsync(int modelId, int? ownerUserId = null);

    public Task RateTopicAsync(int userId, int topicId, int stars);

    public Task RemoveRateAsync(int userId, int topicId);

    public Task<bool> CheckTopicOwnerAsync(int topicId, int userId);

    public Task<TopicDialogModel> GetByIdWithDetailsAsync(params object[] keys);

    public Task RemoveRateWithUserOwnerCheck(int ownerUserId, int topicId);
}
