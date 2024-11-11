namespace WebApp.Infrastructure.Models.UserModels;

using System.Text.Json.Serialization;

public class UserUpdateModel
{
    [JsonIgnore]
    public int Id { get; set; }

    public string? Nickname { get; set; }

    public string? Email { get; set; }
}
