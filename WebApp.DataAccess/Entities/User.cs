namespace WebApp.DataAccess.Entities;

public class User : BaseEntity
{
    public int Id { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public UserRole Role { get; set; }

    public ICollection<Topic> Topics { get; set; } = new List<Topic>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public ICollection<MessageLike> Likes { get; set; } = new List<MessageLike>();

    public ICollection<TopicStars> Stars { get; set; } = new List<TopicStars>();

    public ICollection<Report> Reports { get; set; } = new List<Report>();
}

public enum UserRole
{
    User,
    Admin,
    Moderator,
}
