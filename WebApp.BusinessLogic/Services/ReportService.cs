using AutoMapper;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models;

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

    public async Task<IEnumerable<ReportModel>> GetAllAsync()
    {
        var reportEntities = await this.unitOfWork.ReportRepository.GetAllAsync();
        var reportModels = reportEntities.Select(r => this.mapper.MapWithExceptionHandling<ReportModel>(r));

        return reportModels;
    }

    public async Task<ReportModel> GetByIdAsync(int id)
    {
        var reportEntity = await this.unitOfWork.ReportRepository.GetByIdAsync(id);
        var reportModel = this.mapper.MapWithExceptionHandling<ReportModel>(reportEntity);

        return reportModel;
    }

    public async Task AddAsync(ReportCreateModel createModel)
    {
        ForumException.ThrowIfReportCreateModelIsNotCorrect(createModel);

        var report = this.mapper.MapWithExceptionHandling<Report>(createModel);

        await this.unitOfWork.ReportRepository.AddAsync(report);
        await this.unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(ReportModel model)
    {
        ForumException.ThrowIfReportModelIsNotCorrect(model);

        var report = this.mapper.MapWithExceptionHandling<Report>(model);
        this.unitOfWork.ReportRepository.Update(report);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await this.unitOfWork.ReportRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }
}
