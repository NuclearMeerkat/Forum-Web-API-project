namespace WebApp.Infrastructure.Interfaces.IServices;

using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Models.ReportModels;

public interface IReportService : ICrud<ReportSummaryModel, ReportCreateModel, ReportUpdateModel, ReportQueryParametersModel, CompositeKey>
{
    public Task<IEnumerable<ReportSummaryModel>> GetReportsForTopicAsync(int topicId);
}
