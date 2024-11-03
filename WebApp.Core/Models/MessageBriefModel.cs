﻿namespace WebApp.Core.Models;

public class MessageBriefModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public int LikesCounter { get; set; }
}
