using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Entities;
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

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var userEntities = await this.unitOfWork.UserRepository.GetAllAsync();
        var userModels = userEntities.Select(u => this.mapper.MapWithExceptionHandling<UserModel>(u));

        return userModels;
    }

    public async Task<UserModel> GetByIdAsync(params object[] keys)
    {
        var userEntity = await this.unitOfWork.UserRepository.GetByIdAsync(keys);
        var userModel = this.mapper.MapWithExceptionHandling<UserModel>(userEntity);

        return userModel;
    }

    public async Task AddAsync(UserCreateModel model)
    {
        ForumException.ThrowIfUserCreateModelIsNotCorrect(model);

        var user = this.mapper.MapWithExceptionHandling<User>(model);

        await this.unitOfWork.UserRepository.AddAsync(user);
        await this.unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(UserCreateModel model)
    {
        ForumException.ThrowIfNull(model);

        var user = this.mapper.MapWithExceptionHandling<User>(model);
        this.unitOfWork.UserRepository.Update(user);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await this.unitOfWork.UserRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }
}
