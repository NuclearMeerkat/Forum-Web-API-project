namespace WebApp.Infrastructure.Models.TopicModels;

using WebApp.Infrastructure.Models.MessageModels;

public class TopicDialogModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public double AverageStars { get; set; }

    public string CreatorNickname { get; set; }

    public DateTime CreatedAt { get; set; }

    public IEnumerable<MessageBriefModel> Messages { get; set; }
}
