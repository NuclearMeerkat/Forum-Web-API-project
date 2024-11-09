﻿using WebApp.Infrastructure.Entities;

namespace WebApp.Infrastructure.Interfaces.IRepositories;

public interface IMessageRepository : IRepository<Message>
{
    public Task<Message> GetByIdWithDetailsAsync(int id);

    public Task<IEnumerable<Message>> GetAllWithDetailsAsync();
    Task<IEnumerable<Message>> GetByTopicId(int topicId);
}