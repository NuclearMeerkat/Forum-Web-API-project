using System.Text.Json.Serialization;

namespace WebApp.Core.Models.MessageModels;

public class MessageUpdateModel
{
    [JsonIgnore]
    public int Id { get; set; }

    public string Content { get; set; }
}
