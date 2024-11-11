namespace WebApp.Infrastructure.Models.TopicModels;

using System.Text.Json.Serialization;

public class DeleteRateModel
{
    [JsonIgnore]
    public int UserId { get; set; }

    public int TopicId { get; set; }
}
