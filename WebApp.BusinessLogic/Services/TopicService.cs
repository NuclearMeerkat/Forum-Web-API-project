using AutoMapper;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.MessageModels;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.BusinessLogic.Services;

public class TopicService : ITopicService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    private static double CalculateActivity(ICollection<MessageModel> messages)
    {
        double decayConstant = 0.1; // Adjust based on desired decay rate

        return messages
            .OrderByDescending(m => m.CreatedAt)
            .Take(100) // Only the last 100 messages
            .Select(m => Math.Exp(-decayConstant * (DateTime.Now - m.CreatedAt).TotalDays))
            .Sum();
    }

    public TopicService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TopicSummaryModel>> GetAllAsync(TopicQueryParametersModel? queryParameters = null)
    {
        if (queryParameters == null || queryParameters.RetrieveAll)
        {
            var allTopics = await this.unitOfWork.TopicRepository.GetAllAsync();
            return allTopics.Select(m => this.mapper.Map<TopicSummaryModel>(m));
        }

        IEnumerable<Topic> query = new List<Topic>();

        // Start with the query for all topics
        if (queryParameters.Size > 0)
        {
            query = await this.unitOfWork.TopicRepository.GetRangeAsync(
                (queryParameters.Page - 1) * queryParameters.Size, queryParameters.Size);
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
            "Date" => queryParameters.Ascending
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt),
            "Author" => queryParameters.Ascending
                ? query.OrderBy(t => t.User.Nickname)
                : query.OrderByDescending(t => t.User.Nickname),
            _ => queryParameters.Ascending ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title)
        };

        // Execute the query and project results to TopicDto
        var topics = query.Select(t => this.mapper.MapWithExceptionHandling<TopicSummaryModel>(t));

        foreach (var topic in query)
        {
            if (topic.Messages is null || topic.Messages.Count == 0)
            {
                topics.First(t => t.Id == topic.Id).ActivityScore = 0;
            }
            else
            {
                var topicMessages = topic.Messages.Select(m => this.mapper.MapWithExceptionHandling<MessageModel>(m));

                topic.ActivityScore =
                    CalculateActivity(topicMessages.ToList());
            }
        }

        return topics;
    }

    public async Task<TopicSummaryModel> GetByIdAsync(params object[] keys)
    {
        if (!this.unitOfWork.TopicRepository.IsExist(keys))
        {
            throw new ForumException($"Topic with this id is not found");
        }

        var topicEntity = await this.unitOfWork.TopicRepository.GetWithDetailsAsync((int)keys[0]);
        var topicModel = this.mapper.MapWithExceptionHandling<TopicSummaryModel>(topicEntity);

        if (topicEntity.Messages.Count == 0 || topicEntity.Messages is null)
        {
            topicModel.ActivityScore = 0;
        }
        else
        {
            var topicMessages = topicEntity.Messages.Select(m => this.mapper.MapWithExceptionHandling<MessageModel>(m));
            topicModel.ActivityScore = CalculateActivity(topicMessages.ToList());
        }

        return topicModel;
    }

    public async Task<TopicDialogModel> GetByIdWithDetailsAsync(params object[] keys)
    {
        if (!this.unitOfWork.TopicRepository.IsExist(keys))
        {
            throw new ForumException($"Topic with this id is not found");
        }

        var topicEntity = await this.unitOfWork.TopicRepository.GetWithDetailsAsync((int)keys[0]);
        var topicModel = this.mapper.MapWithExceptionHandling<TopicDialogModel>(topicEntity);

        return topicModel;
    }

    public async Task<int> AddAsync(AdminTopicCreateModel model)
    {
        ForumException.ThrowIfTopicCreateModelIsNotCorrect(model);

        if (!this.unitOfWork.UserRepository.IsExist(model.UserId))
        {
            throw new ForumException("User with this id does not exist");
        }

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

    public async Task<bool> CheckTopicOwner(int topicId, int userId)
    {
        Topic topic;
        try
        {
            topic = await this.unitOfWork.TopicRepository.GetByIdAsync(topicId);
            if (topic is null)
            {
                throw new InvalidOperationException();
            }
        }
        catch (InvalidOperationException)
        {
            throw new ForumException("Topic with this id is not found");
        }

        if (userId == topic.UserId)
        {
            return true;
        }

        return false;
    }

    public async Task DeleteAsync(int modelId)
    {
        if (!this.unitOfWork.TopicRepository.IsExist(modelId))
        {
            throw new ForumException("Topic with this id does not exist");
        }

        await this.unitOfWork.TopicRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task RateTopic(int userId, int topicId, int stars)
    {
        if (!this.unitOfWork.TopicRepository.IsExist(topicId))
        {
            throw new ForumException("Topic with this id is not found");
        }

        if (!this.unitOfWork.UserRepository.IsExist(userId))
        {
            throw new ForumException("User with this id is not found");
        }

        try
        {
            // Check if the user has already rated this topic
            var existingRating = await this.unitOfWork.TopicStarsRepository.GetByIdAsync(userId, topicId);
            existingRating.StarCount = stars;
            this.unitOfWork.TopicStarsRepository.Update(existingRating);
        }
        catch (NullReferenceException)
        {
            await this.unitOfWork.TopicStarsRepository.AddAsync(new TopicStars
            {
                UserId = userId,
                TopicId = topicId,
                StarCount = stars,
            });
        }

        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveRate(int userId, int topicId)
    {
        if (!this.unitOfWork.TopicStarsRepository.IsExist(userId, topicId))
        {
            throw new InvalidOperationException("Stars with such id for this topic does not exist");
        }

        await this.unitOfWork.TopicStarsRepository.DeleteByIdAsync(userId, topicId);
        await this.unitOfWork.SaveAsync();
    }
}
