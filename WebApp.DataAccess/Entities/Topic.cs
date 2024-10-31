namespace WebApp.DataAccess.Entities;

public class Topic : BaseEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int UserId { get; set; }

    public User User { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public ICollection<TopicStars> Stars { get; set; } = new List<TopicStars>();
}
