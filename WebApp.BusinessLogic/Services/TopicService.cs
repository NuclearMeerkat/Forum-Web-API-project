using AutoMapper;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models;

namespace WebApp.BusinessLogic.Services;

public class TopicService : ITopicService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    private double CalculateActivity(ICollection<MessageModel> messages)
    {
        double decayConstant = 0.1; // Adjust based on desired decay rate

        return messages
            .OrderByDescending(m => m.CreatedAt)
            .Take(100) // Only the last 100 messages
            .Select(m => Math.Exp(-decayConstant * (double)(DateTime.Now - m.CreatedAt).TotalDays))
            .Sum();
    }

    public TopicService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TopicModel>> GetAllAsync()
    {
        var topicEntities = await this.unitOfWork.TopicRepository.GetAllAsync();
        var topicModels = topicEntities.Select(t => this.mapper.MapWithExceptionHandling<TopicModel>(t));
        var allAsync = topicModels.ToList();
        foreach (var topicModel in allAsync)
        {
            topicModel.ActivityScore = this.CalculateActivity(topicModel.Messages);
        }

        return allAsync;
    }

    public async Task<TopicModel> GetByIdAsync(params object[] keys)
    {
        var topicEntity = await this.unitOfWork.TopicRepository.GetByIdAsync(keys);
        var topicModel = this.mapper.MapWithExceptionHandling<TopicModel>(topicEntity);

        topicModel.ActivityScore = this.CalculateActivity(topicModel.Messages);

        return topicModel;
    }

    public async Task AddAsync(TopicCreateModel createModel)
    {
        ForumException.ThrowIfTopicCreateModelIsNotCorrect(createModel);

        var topic = this.mapper.MapWithExceptionHandling<Topic>(createModel);

        await this.unitOfWork.TopicRepository.AddAsync(topic);
        await this.unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(TopicModel model)
    {
        ForumException.ThrowIfNull(model);
        var topic = this.mapper.MapWithExceptionHandling<Topic>(model);
        this.unitOfWork.TopicRepository.Update(topic);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await this.unitOfWork.TopicRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task RateTopic(int userId, int topicId, int stars)
    {
        await this.unitOfWork.TopicStarsRepository.AddAsync(new TopicStars
        {
            UserId = userId, TopicId = topicId, StarCount = stars,
        });

        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveRate(int userId, int topicId)
    {
        await this.unitOfWork.TopicStarsRepository.DeleteByIdAsync(userId, topicId);
        await this.unitOfWork.SaveAsync();
    }

    public Task GetAverageRating(int topicId)
    {
        throw new NotImplementedException();
    }
}
