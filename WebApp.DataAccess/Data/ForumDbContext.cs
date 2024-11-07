using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using WebApp.Core.Entities;

namespace WebApp.DataAccess.Data;

public class ForumDbContext : DbContext
{
    // Todo: connection string and config at all

    public ForumDbContext(DbContextOptions<ForumDbContext> options)
        : base(options)
    {
    }

    public ForumDbContext()
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Topic> Topics { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<MessageLike> Likes { get; set; }

    public DbSet<TopicStars> TopicStars { get; set; }

    public DbSet<Report> Reports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=ForumDb;Trusted_Connection=True;");
        optionsBuilder.UseSqlServerTriggers();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (modelBuilder is null)
        {
            throw new ArgumentNullException($"{modelBuilder} is null here");
        }

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Nickname)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Topic)
            .WithMany(t => t.Messages)
            .HasForeignKey(m => m.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Message>()
            .HasOne(m => m.ParentMessage)
            .WithMany(m => m.Replies)
            .HasForeignKey(m => m.ParentMessageId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Message>()
            .HasMany(t => t.Likes)
            .WithOne(m => m.Message)
            .HasForeignKey(m => m.MessageId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Message>()
            .Property(m => m.IsEdited)
            .HasDefaultValue(false);
        modelBuilder.Entity<Message>()
            .Property(m => m.LikesCounter)
            .HasDefaultValue(0);


        modelBuilder.Entity<MessageLike>()
            .HasKey(l => new { l.UserId, l.MessageId });
        modelBuilder.Entity<MessageLike>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MessageLike>()
            .HasOne(l => l.Message)
            .WithMany(m => m.Likes)
            .HasForeignKey(l => l.MessageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TopicStars>()
            .HasKey(l => new { l.UserId, l.TopicId });
        modelBuilder.Entity<TopicStars>()
            .HasOne(r => r.User)
            .WithMany(u => u.Stars)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TopicStars>()
            .HasOne(r => r.Topic)
            .WithMany(t => t.Stars)
            .HasForeignKey(r => r.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Report>()
            .HasKey(r => new { r.UserId, r.MessageId });
        modelBuilder.Entity<Report>()
            .HasOne(r => r.Message)
            .WithMany(m => m.Reports)
            .HasForeignKey(r => r.MessageId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Report>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Report>()
            .Property(r => r.Status)
            .HasConversion<string>();
        modelBuilder.Entity<Report>()
            .HasOne(r => r.Message)
            .WithMany(m => m.Reports)
            .HasForeignKey(r => r.MessageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Topic>()
            .HasMany(t => t.Messages)
            .WithOne(m => m.Topic)
            .HasForeignKey(m => m.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Topic>()
            .HasMany(t => t.Stars)
            .WithOne(m => m.Topic)
            .HasForeignKey(m => m.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MessageLike>()
            .AfterInsert(trigger => trigger
                .Action(action => action
                    .Update<Message>(
                        (tableRefs, message) =>
                            message.Id ==
                            tableRefs.New.MessageId, // Condition for updating the Message with matching Id
                        (tableRefs, message) => new Message
                        {
                            LikesCounter = message.LikesCounter + 1, // Increment the LikesCounter field
                        })));
    }
}
