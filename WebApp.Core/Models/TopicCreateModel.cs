namespace WebApp.Core.Models;

public class TopicCreateModel
{
    public int UserId { get; set; }

    public string TitleId { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
