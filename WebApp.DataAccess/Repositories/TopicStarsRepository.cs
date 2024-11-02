using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Entities;
using WebApp.DataAccess.Interfaces;

namespace WebApp.DataAccess.Repositories;

public class TopicStarsRepository : GenericRepository<TopicStars>, ITopicStarsRepository
{
    public TopicStarsRepository(ForumDbContext context)
        : base(context)
    {
    }

    public async Task<double> GetAverageStarsForTopicAsync(int topicId)
    {
        return await this.context.TopicStars
            .Where(ts => ts.TopicId == topicId)
            .AverageAsync(ts => ts.StarCount);
    }
}
