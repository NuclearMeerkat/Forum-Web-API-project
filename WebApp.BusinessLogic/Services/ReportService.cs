using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.ReportModels;

namespace WebApp.BusinessLogic.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<ReportSummaryModel>> GetAllAsync(
        ReportQueryParametersModel? queryParameters = default)
    {
        if (queryParameters == null || queryParameters.RetrieveAll)
        {
            var allReports = await this.unitOfWork.TopicRepository.GetAllAsync();
            return allReports.Select(m => this.mapper.Map<ReportSummaryModel>(m));
        }

        IEnumerable<Report> query = new List<Report>();

        // Start with the query for all topics
        if (queryParameters.Size > 0)
        {
            query = await this.unitOfWork.ReportRepository.GetRangeAsync(
                (queryParameters.Page - 1) * queryParameters.Size, queryParameters.Size);
        }

        // Apply filtering if a search term is provided
        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            query = query.Where(t =>
                queryParameters.Search.Contains(t.Reason, StringComparison.InvariantCulture) ||
                t.User.Nickname.Contains(queryParameters.Search, StringComparison.InvariantCulture) ||
                t.Message.Content.Contains(queryParameters.Search, StringComparison.InvariantCulture));
        }

        // Apply sorting
        query = queryParameters.SortBy switch
        {
            "Reason" => queryParameters.Ascending
                ? query.OrderBy(t => t.Reason)
                : query.OrderByDescending(t => t.Reason),
            "Date" => queryParameters.Ascending
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt),
            "User" => queryParameters.Ascending
                ? query.OrderBy(t => t.User.Nickname)
                : query.OrderByDescending(t => t.User.Nickname),
            _ => queryParameters.Ascending ? query.OrderBy(t => t.Reason) : query.OrderByDescending(t => t.Reason)
        };

        // Execute the query and project results to TopicDto
        var reports = query.Select(t => this.mapper.MapWithExceptionHandling<ReportSummaryModel>(t));

        return reports;
    }

    public async Task<ReportSummaryModel> GetByIdAsync(params object[] keys)
    {
        if (!this.unitOfWork.ReportRepository.IsExist(keys))
        {
            throw new ForumException("There is no Report with the given keys.");
        }

        var reportEntity = await this.unitOfWork.ReportRepository.GetByIdAsync(keys);

        var reportModel = this.mapper.MapWithExceptionHandling<ReportSummaryModel>(reportEntity);

        return reportModel;
    }

    public async Task<ReportCompositeKey> AddAsync(ReportCreateModel model)
    {
        ForumException.ThrowIfReportCreateModelIsNotCorrect(model);

        if (!this.unitOfWork.UserRepository.IsExist(model.UserId))
        {
            throw new ForumException("User with this id does not exist");
        }

        if (!this.unitOfWork.MessageRepository.IsExist(model.MessageId))
        {
            throw new ForumException("Message with this id does not exist");
        }

        var report = this.mapper.MapWithExceptionHandling<Report>(model);

        try
        {
            await this.unitOfWork.ReportRepository.AddAsync(report);
        }
        catch (DbUpdateException)
        {
            throw new DbUpdateException("An error occured while adding a report");
        }

        ReportCompositeKey key = new ReportCompositeKey() { UserId = model.UserId, MessageId = model.MessageId };
        await this.unitOfWork.SaveAsync();

        return key;
    }

    public async Task UpdateAsync(ReportUpdateModel model)
    {
        ForumException.ThrowIfNull(model);

        Report existingReport;
        try
        {
            existingReport = await this.unitOfWork.ReportRepository.GetByIdAsync(model.UserId, model.MessageId);
            if (existingReport == null)
            {
                throw new NullReferenceException();
            }
        }
        catch (Exception)
        {
            throw new ForumException("Report with this id is not found");
        }
#pragma warning disable CS8629 // Nullable value type may be null.
        existingReport.Status = (ReportStatus)model.Status;
#pragma warning restore CS8629 // Nullable value type may be null.
        this.unitOfWork.ReportRepository.Update(existingReport);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(ReportCompositeKey modelId)
    {
        ArgumentNullException.ThrowIfNull(modelId);

        if (!this.unitOfWork.ReportRepository.IsExist(modelId.UserId, modelId.MessageId))
        {
            throw new ForumException("Report with this id is not found");
        }

        await this.unitOfWork.ReportRepository.DeleteByIdAsync(modelId.UserId, modelId.MessageId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ReportSummaryModel>> GetReportsForTopicAsync(int topicId)
    {
        if (!this.unitOfWork.TopicRepository.IsExist(topicId))
        {
            throw new ForumException("Topic with this id does not exist");
        }

        var reportEntities = await this.unitOfWork.ReportRepository.GetReportsForTopicAsync(topicId);
        var reportModels = reportEntities.Select(r => this.mapper.MapWithExceptionHandling<ReportSummaryModel>(r));

        return reportModels;
    }
}
