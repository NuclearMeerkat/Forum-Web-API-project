using System.Text.Json.Serialization;
using WebApp.Core.Enums;

namespace WebApp.Core.Models.UserModels;

public class UserCreateModel
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Nickname { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? ProfilePictureUrl { get; set; }
}
