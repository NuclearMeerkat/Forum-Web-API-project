﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.DataAccess.Data;

#nullable disable

namespace WebApp.DataAccess.Migrations
{
    [DbContext(typeof(ForumDbContext))]
    partial class ForumDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApp.Infrastructure.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEdited")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("LikesCounter")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int?>("ParentMessageId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentMessageId");

                    b.HasIndex("TopicId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages", (string)null);
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.MessageLike", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "MessageId");

                    b.HasIndex("MessageId");

                    b.ToTable("Likes", null, t =>
                        {
                            t.HasTrigger("LC_TRIGGER_AFTER_DELETE_MESSAGELIKE");

                            t.HasTrigger("LC_TRIGGER_AFTER_INSERT_MESSAGELIKE");
                        });

                    b
                        .HasAnnotation("LC_TRIGGER_AFTER_DELETE_MESSAGELIKE", "CREATE TRIGGER LC_TRIGGER_AFTER_DELETE_MESSAGELIKE ON \"Likes\" AFTER Delete AS\r\nBEGIN\r\n  DECLARE @OldMessageId INT\r\n  DECLARE DeletedMessageLikeCursor CURSOR LOCAL FOR SELECT MessageId FROM Deleted\r\n  OPEN DeletedMessageLikeCursor\r\n  FETCH NEXT FROM DeletedMessageLikeCursor INTO @OldMessageId\r\n  WHILE @@FETCH_STATUS = 0\r\n  BEGIN\r\n    UPDATE \"Messages\"\r\n    SET \"LikesCounter\" = \"Messages\".\"LikesCounter\" - 1\r\n    WHERE \"Messages\".\"Id\" = @OldMessageId;\r\n  FETCH NEXT FROM DeletedMessageLikeCursor INTO @OldMessageId\r\n  END\r\n  CLOSE DeletedMessageLikeCursor DEALLOCATE DeletedMessageLikeCursor\r\nEND")
                        .HasAnnotation("LC_TRIGGER_AFTER_INSERT_MESSAGELIKE", "CREATE TRIGGER LC_TRIGGER_AFTER_INSERT_MESSAGELIKE ON \"Likes\" AFTER Insert AS\r\nBEGIN\r\n  DECLARE @NewMessageId INT\r\n  DECLARE InsertedMessageLikeCursor CURSOR LOCAL FOR SELECT MessageId FROM Inserted\r\n  OPEN InsertedMessageLikeCursor\r\n  FETCH NEXT FROM InsertedMessageLikeCursor INTO @NewMessageId\r\n  WHILE @@FETCH_STATUS = 0\r\n  BEGIN\r\n    UPDATE \"Messages\"\r\n    SET \"LikesCounter\" = \"Messages\".\"LikesCounter\" + 1\r\n    WHERE \"Messages\".\"Id\" = @NewMessageId;\r\n  FETCH NEXT FROM InsertedMessageLikeCursor INTO @NewMessageId\r\n  END\r\n  CLOSE InsertedMessageLikeCursor DEALLOCATE InsertedMessageLikeCursor\r\nEND")
                        .HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.Report", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReviewedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "MessageId");

                    b.HasIndex("MessageId");

                    b.ToTable("Reports", (string)null);
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Topics", (string)null);
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.TopicStars", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("StarCount")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TopicId");

                    b.HasIndex("TopicId");

                    b.ToTable("TopicStars", (string)null);
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Nickname")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.Message", b =>
                {
                    b.HasOne("WebApp.Infrastructure.Entities.Message", "ParentMessage")
                        .WithMany("Replies")
                        .HasForeignKey("ParentMessageId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("WebApp.Infrastructure.Entities.Topic", "Topic")
                        .WithMany("Messages")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebApp.Infrastructure.Entities.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ParentMessage");

                    b.Navigation("Topic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.MessageLike", b =>
                {
                    b.HasOne("WebApp.Infrastructure.Entities.Message", "Message")
                        .WithMany("Likes")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebApp.Infrastructure.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.Report", b =>
                {
                    b.HasOne("WebApp.Infrastructure.Entities.Message", "Message")
                        .WithMany("Reports")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebApp.Infrastructure.Entities.User", "User")
                        .WithMany("Reports")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.Topic", b =>
                {
                    b.HasOne("WebApp.Infrastructure.Entities.User", "User")
                        .WithMany("Topics")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.TopicStars", b =>
                {
                    b.HasOne("WebApp.Infrastructure.Entities.Topic", "Topic")
                        .WithMany("Stars")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebApp.Infrastructure.Entities.User", "User")
                        .WithMany("Stars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Topic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.Message", b =>
                {
                    b.Navigation("Likes");

                    b.Navigation("Replies");

                    b.Navigation("Reports");
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.Topic", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Stars");
                });

            modelBuilder.Entity("WebApp.Infrastructure.Entities.User", b =>
                {
                    b.Navigation("Likes");

                    b.Navigation("Messages");

                    b.Navigation("Reports");

                    b.Navigation("Stars");

                    b.Navigation("Topics");
                });
#pragma warning restore 612, 618
        }
    }
}
