﻿using WebApp.Infrastructure.Entities;

namespace WebApp.Infrastructure.Interfaces.IRepositories;

public interface ITopicRepository : IRepository<Topic>
{
    public Task<Topic> GetWithDetailsAsync(int id);

    public Task<IEnumerable<Topic>> GetAllWithDetailsAsync();

    public Task<IEnumerable<Topic>> GetRangeAsync(int skip, int take);
}