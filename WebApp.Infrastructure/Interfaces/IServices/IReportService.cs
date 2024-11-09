using WebApp.Infrastructure.Models.TopicModels;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Models.ReportModels;

namespace WebApp.Infrastructure.Interfaces.IServices;
public interface IReportService : ICrud<ReportSummaryModel, ReportCreateModel, ReportUpdateModel, ReportQueryParametersModel, CompositeKey>
{
    public Task<IEnumerable<ReportSummaryModel>> GetReportsForTopicAsync(int topicId);
}
