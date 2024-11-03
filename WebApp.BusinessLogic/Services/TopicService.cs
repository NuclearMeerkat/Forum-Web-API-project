using AutoMapper;
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

    public Task<IEnumerable<TopicModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TopicModel> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(TopicModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TopicModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int modelId)
    {
        throw new NotImplementedException();
    }

    public Task RateTopic(int userId, int topicId, int stars)
    {
        throw new NotImplementedException();
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
