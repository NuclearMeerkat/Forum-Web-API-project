using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApp.Infrastructure.Models.MessageModels;

public class MessageUpdateModel
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore] public bool IsEdited { get; set; } = true;
    public string Content { get; set; }
}
