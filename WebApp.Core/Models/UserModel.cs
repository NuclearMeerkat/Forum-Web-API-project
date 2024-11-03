﻿using WebApp.Core.Enums;

namespace WebApp.Core.Models;

public class UserModel
{
    public int Id { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public ICollection<TopicModel> ParticipatedTopics { get; set; } = new List<TopicModel>();

    public ICollection<TopicModel> OwnedTopics { get; set; } = new List<TopicModel>();

    public ICollection<MessageModel> Messages { get; set; } = new List<MessageModel>();

    public ICollection<ReportModel> Reports { get; set; } = new List<ReportModel>();
}
