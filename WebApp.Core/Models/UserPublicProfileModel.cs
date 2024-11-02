namespace WebApp.Core.Models;

public class UserPublicProfileModel
{
    public int Id { get; set; }

    public string Nickname { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public DateTime? LastLogin { get; set; }
}
