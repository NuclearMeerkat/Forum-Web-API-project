using System.Text.Json.Serialization;
using WebApp.Infrastructure.Enums;

namespace WebApp.Infrastructure.Models.UserModels;

public class UserRegisterModel
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Nickname { get; set; }

    public string PasswordHash { get; set; }
    public string Email { get; set; }

    [JsonIgnore]
    public UserRole Role { get; set; } = 0;

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
