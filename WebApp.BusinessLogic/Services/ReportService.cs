using AutoMapper;
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

    public Task<IEnumerable<ReportModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ReportModel> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(ReportModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(ReportModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int modelId)
    {
        throw new NotImplementedException();
    }
}
