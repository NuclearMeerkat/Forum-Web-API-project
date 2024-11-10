using System.Text.Json.Serialization;

namespace WebApp.Infrastructure.Models.UserModels;

public class UserUpdateModel
{
    [JsonIgnore]
    public int Id { get; set; }

    public string? Nickname { get; set; }

    public string? Email { get; set; }
}
