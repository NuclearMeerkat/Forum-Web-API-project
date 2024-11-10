namespace WebApp.Infrastructure.Models.UserModels;

public class UserPublicProfileModel
{
    public int Id { get; set; }

    public string Nickname { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}
