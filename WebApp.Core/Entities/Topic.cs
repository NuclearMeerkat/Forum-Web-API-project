namespace WebApp.Core.Entities;

public class Topic : BaseEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public double AverageStars { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public ICollection<TopicStars> Stars { get; set; } = new List<TopicStars>();
}
