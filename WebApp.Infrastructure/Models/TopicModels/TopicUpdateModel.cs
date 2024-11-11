namespace WebApp.Infrastructure.Models.TopicModels;

using System.Text.Json.Serialization;

public class TopicUpdateModel
{
    [JsonIgnore]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
}
