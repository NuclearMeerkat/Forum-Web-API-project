namespace WebApp.Core.Models;

public class TopicDtoModel
{
    public int UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
