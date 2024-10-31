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
}
