namespace WebApp.Infrastructure.Models.TopicModels;

using System.Text.Json.Serialization;

public class AdminTopicCreateModel
{
    [JsonIgnore]
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
