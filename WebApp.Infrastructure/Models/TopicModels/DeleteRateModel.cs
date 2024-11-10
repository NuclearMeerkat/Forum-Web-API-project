using System.Text.Json.Serialization;

namespace WebApp.Infrastructure.Models.TopicModels;

public class DeleteRateModel
{
    [JsonIgnore]
    public int UserId { get; set; }

    public int TopicId { get; set; }
}
