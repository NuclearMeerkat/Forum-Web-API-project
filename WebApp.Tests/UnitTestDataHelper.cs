using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Enums;

namespace WebApp.Tests;
public static class UnitTestDataHelper
{
    public static DbContextOptions<ForumDbContext> GetUnitTestDbOptions()
    {
        var options = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ForumDbContext(options);
        SeedData(context);

        return options;
    }

    public static void SeedData(ForumDbContext context)
    {
        // Seed Users
        context.Users.AddRange(
            new User { Id = 1, Nickname = "JaneDoe", Email = "jane.doe@example.com", ProfilePictureUrl = "/images/jane.jpg", Role = UserRole.User, PasswordHash = "password1" },
            new User { Id = 2, Nickname = "JohnSmith", Email = "john.smith@example.com", ProfilePictureUrl = "/images/john.jpg", Role = UserRole.Moderator, PasswordHash = "password2" },
            new User { Id = 3, Nickname = "AliceBlue", Email = "alice.blue@example.com", ProfilePictureUrl = "/images/alice.jpg", Role = UserRole.Admin, PasswordHash = "password3" }
        );

        // Seed Topics
        context.Topics.AddRange(
            new Topic { Id = 1, Title = "Welcome to the Forum", Description = "Introduce yourself here!", UserId = 1 },
            new Topic { Id = 2, Title = "General Discussion", Description = "Talk about anything here!", UserId = 2 },
            new Topic { Id = 3, Title = "Tech Talk", Description = "Discuss the latest in technology!", UserId = 3 }
        );

        // Seed Messages (with replies and likes)
        context.Messages.AddRange(
            new Message { Id = 1, Content = "Hello everyone!", UserId = 1, TopicId = 1, LikesCounter = 2 },
            new Message { Id = 2, Content = "Nice to meet you all!", UserId = 2, TopicId = 1, LikesCounter = 1, ParentMessageId = 1 }, // Reply to Message 1
            new Message { Id = 3, Content = "What's new in tech?", UserId = 3, TopicId = 3, LikesCounter = 0 }
        );

        // Seed MessageLikes
        context.Likes.AddRange(
            new MessageLike { MessageId = 1, UserId = 2 },
            new MessageLike { MessageId = 1, UserId = 3 },
            new MessageLike { MessageId = 2, UserId = 1 }
        );

        // Seed TopicStars
        context.TopicStars.AddRange(
            new TopicStars { StarCount = 4, UserId = 1, TopicId = 1 },
            new TopicStars { StarCount = 5, UserId = 2, TopicId = 2 },
            new TopicStars { StarCount = 3, UserId = 3, TopicId = 3 }
        );

        // Seed Reports
        context.Reports.AddRange(
            new Report { UserId = 3, MessageId = 2, Reason = "Spam content", Status = ReportStatus.Pending, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new Report { UserId = 2, MessageId = 1, Reason = "Inappropriate language", Status = ReportStatus.Resolved, CreatedAt = DateTime.UtcNow.AddDays(-1), ReviewedAt = DateTime.UtcNow }
        );

        context.SaveChanges();
    }
}
