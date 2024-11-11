namespace WebApp.Infrastructure.Interfaces.IServices;

using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Models.ReportModels;

public interface IReportService
{
    Task<IEnumerable<ReportSummaryModel>> GetAllAsync(ReportQueryParametersModel? queryParameters = default);

    Task<ReportSummaryModel> GetByIdAsync(params object[] keys);

    Task<CompositeKey> AddAsync(ReportCreateModel model);

    Task UpdateAsync(ReportUpdateModel model);

    Task DeleteAsync(CompositeKey modelId);

    public Task<IEnumerable<ReportSummaryModel>> GetReportsForTopicAsync(int topicId);
}
