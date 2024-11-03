using AutoMapper;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models;

namespace WebApp.BusinessLogic.Services;
public class MessageService : IMessageService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<IEnumerable<MessageModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<MessageModel> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(MessageModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(MessageModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int modelId)
    {
        throw new NotImplementedException();
    }

    public Task AddLike(int userId, int messageId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveLike(int userId, int messageId)
    {
        throw new NotImplementedException();
    }
}
