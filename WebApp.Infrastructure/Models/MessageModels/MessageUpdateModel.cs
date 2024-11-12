namespace WebApp.Infrastructure.Models.MessageModels;

using System.Text.Json.Serialization;

public class MessageUpdateModel
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    public bool IsEdited { get; set; } = true;

    [JsonIgnore]
    public int UserId { get; set; }

    public string Content { get; set; }
}
