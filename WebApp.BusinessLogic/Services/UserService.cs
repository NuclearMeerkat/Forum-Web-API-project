using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models;

namespace WebApp.BusinessLogic.Services;
public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<IEnumerable<UserModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(UserModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UserModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int modelId)
    {
        throw new NotImplementedException();
    }
}
