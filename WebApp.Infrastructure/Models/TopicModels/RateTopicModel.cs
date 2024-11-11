namespace WebApp.Infrastructure.Models.TopicModels;

using System.Text.Json.Serialization;

public class RateTopicModel
{
    [JsonIgnore]
    public int UserId { get; set; }

    public int TopicId { get; set; }

    public int Stars { get; set; }
}
