﻿namespace WebApp.Infrastructure.Interfaces.IRepositories;

using WebApp.Infrastructure.Entities;

public interface IUserRepository : IRepository<User>
{
    public Task<User> GetWithDetailsAsync(int id);

    public Task<IEnumerable<User>> GetAllWithDetailsAsync();

    public Task<User> GetByEmailAsync(string email);

    public Task<IEnumerable<User>> GetRange(int skip, int take);
}
