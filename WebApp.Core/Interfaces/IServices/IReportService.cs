using WebApp.Core.Entities;
using WebApp.Core.Models.ReportModels;
using WebApp.Core.Models.TopicModels;

namespace WebApp.Core.Interfaces.IServices;
public interface IReportService : ICrud<ReportModel, ReportCreateModel, ReportUpdateModel, TopicQueryParametersModel, CompositeKey>
{
    public Task<IEnumerable<ReportSummaryModel>> GetReportsForTopicAsync(int topicId);
}
