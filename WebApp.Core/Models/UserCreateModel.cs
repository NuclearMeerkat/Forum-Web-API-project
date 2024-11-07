using WebApp.Core.Enums;

namespace WebApp.Core.Models;

public class UserCreateModel
{
    public int Id { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? ProfilePictureUrl { get; set; }
}
