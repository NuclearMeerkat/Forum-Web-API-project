using AutoMapper;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models.MessageModels;
using WebApp.Core.Models.TopicModels;

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

    public async Task<IEnumerable<TopicSummaryModel>> GetAllAsync(TopicQueryParametersModel queryParameters)
    {
        IEnumerable<Topic> query = new List<Topic>();

        // Start with the query for all topics
        if (queryParameters.Size > 0)
        {
            query = await this.unitOfWork.TopicRepository.GetRangeAsync(queryParameters.Page, queryParameters.Size);
        }

        // Apply filtering if a search term is provided
        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            query = query.Where(t =>
                queryParameters.Search.Contains(t.Title, StringComparison.InvariantCulture) ||
                t.Description.Contains(queryParameters.Search, StringComparison.InvariantCulture));
        }

        // Apply sorting
        query = queryParameters.SortBy switch
        {
            "Title" => queryParameters.Ascending ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title),
            "DateCreated" => queryParameters.Ascending
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt),
            "Author" => queryParameters.Ascending
                ? query.OrderBy(t => t.User.Nickname)
                : query.OrderByDescending(t => t.User.Nickname),
            _ => queryParameters.Ascending ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title)
        };

        // Execute the query and project results to TopicDto
        var topics = query.Select(t => this.mapper.MapWithExceptionHandling<TopicSummaryModel>(t));

        return topics;
    }

    public async Task<TopicSummaryModel> GetByIdAsync(params object[] keys)
    {
        var topicEntity = await this.unitOfWork.TopicRepository.GetByIdAsync(keys);
        var topicModel = this.mapper.MapWithExceptionHandling<TopicSummaryModel>(topicEntity);

        if (topicEntity.Messages.Count == 0 || topicEntity.Messages is null)
        {
            topicModel.ActivityScore = 0;
        }
        else
        {
            var topicMessages = topicEntity.Messages.Select(m => this.mapper.MapWithExceptionHandling<MessageModel>(m));
            topicModel.ActivityScore = this.CalculateActivity((ICollection<MessageModel>)topicMessages);
        }

        return topicModel;
    }

    public async Task<int> AddAsync(TopicCreateModel model)
    {
        ForumException.ThrowIfTopicCreateModelIsNotCorrect(model);

        var topic = this.mapper.MapWithExceptionHandling<Topic>(model);

        int topicId = (int)await this.unitOfWork.TopicRepository.AddAsync(topic);
        await this.unitOfWork.SaveAsync();

        return topicId;
    }

    public async Task UpdateAsync(TopicUpdateModel model)
    {
        ForumException.ThrowIfNull(model);
        var existingTopic = await this.unitOfWork.TopicRepository.GetByIdAsync(model.Id);
        if (existingTopic == null)
        {
            throw new ForumException("Topic with this id is not found");
        }

        this.mapper.Map(model, existingTopic);

        this.unitOfWork.TopicRepository.Update(existingTopic);

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
}
