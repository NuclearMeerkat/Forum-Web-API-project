namespace WebApp.Infrastructure.Models.MessageModels;

using System.Text.Json.Serialization;

public class MessageCreateModel
{
    [JsonIgnore]
    public int UserId { get; set; }

    [JsonIgnore]
    public int TopicId { get; set; }

    public string Content { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? ParentMessageId { get; set; } = null;
}
