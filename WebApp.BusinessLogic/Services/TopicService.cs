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

    public TopicService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TopicModel>> GetAllAsync()
    {
        var topicEntities = await this.unitOfWork.TopicRepository.GetAllAsync();
        var topicModels = topicEntities.Select(t => this.mapper.MapWithExceptionHandling<TopicModel>(t));

        return topicModels;
    }

    public async Task<TopicModel> GetByIdAsync(int id)
    {
        var topicEntity = await this.unitOfWork.TopicRepository.GetByIdAsync(id);
        var topicModel = this.mapper.MapWithExceptionHandling<TopicModel>(topicEntity);

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
        ForumException.ThrowIfTopicModelIsNotCorrect(model);

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
            UserId = userId,
            TopicId = topicId,
            StarCount = stars,
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

    public async Task<IEnumerable<TopicModel>> GetAllTopicsWithActivityScoresAsync()
    {
        throw new NotImplementedException();
        /*var topics = await this.unitOfWork.TopicRepository.GetAllAsync();
        const double lambda = 0.1; // Time decay rate for messages
        var currentDate = DateTime.UtcNow; // Current date for decay calculation

        foreach (var topic in topics)
        {
            // Apply time decay to messages
            double messageScore = topic.Messages.Sum(m => Math.Exp(-lambda * (currentDate - m.CreatedAt).TotalDays));

            // Count likes and unique users without decay
            double likeScore = topic.Messages.SelectMany(m => m.Likes).Count();
            double uniqueUserScore = topic.Messages.Select(m => m.UserId).Distinct().Count();

            // Calculate final activity score with weights
            topic.ActivityScore = (0.2 * messageScore) + (0.2 * likeScore) + (0.6 * uniqueUserScore);
        }

        return topics;*/
    }
}
