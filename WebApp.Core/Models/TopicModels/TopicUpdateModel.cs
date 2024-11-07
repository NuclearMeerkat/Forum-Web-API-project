using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApp.Core.Models.TopicModels;

public class TopicUpdateModel
{
    [JsonIgnore]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
}
