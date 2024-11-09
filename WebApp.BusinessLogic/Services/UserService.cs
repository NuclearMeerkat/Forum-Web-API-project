using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.Auth;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.TopicModels;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IPasswordHasher passwordHasher;
    private readonly IJvtProvider jvtProvider;

    public UserService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IJvtProvider jvtProvider)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        this.jvtProvider = jvtProvider;
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync(TopicQueryParametersModel? queryParameters)
    {
        var userEntities = await this.unitOfWork.UserRepository.GetAllWithDetailsAsync();
        var userModels = userEntities.Select(u => this.mapper.MapWithExceptionHandling<UserModel>(u));

        return userModels;
    }

    public async Task<UserModel> GetByIdAsync(params object[] keys)
    {
        if (!this.unitOfWork.UserRepository.IsExist((int)keys[0]))
        {
            throw new ForumException("User with this Id does not exist");
        }

        var userEntity = await this.unitOfWork.UserRepository.GetByIdAsync(keys);
        var userModel = this.mapper.MapWithExceptionHandling<UserModel>(userEntity);

        return userModel;
    }

    public async Task<int> RegisterAsync(UserRegisterModel model)
    {
        try
        {
            ForumException.ThrowIfNull(model);

            model.PasswordHash = this.passwordHasher.HashPassword(model.PasswordHash);

            var user = this.mapper.MapWithExceptionHandling<User>(model);

            int userId = (int)await this.unitOfWork.UserRepository.AddAsync(user);
            await this.unitOfWork.SaveAsync();

            return userId;
        }
        catch (Exception)
        {
            throw new ForumException("Failed to register user");
        }
    }

    public async Task<string> LoginAsync(UserLoginModel model)
    {
        var user = await this.unitOfWork.UserRepository.GetByEmailAsync(model.Email);

        if (user is null)
        {
            throw new ForumException("Invalid password or email");
        }

        var result = this.passwordHasher.Verify(model.Password, user.PasswordHash);

        if (result == false)
        {
            throw new ForumException("Invalid password or email");
        }

        var token = this.jvtProvider.GenerateToken(user);

        return token;
    }

    public async Task UpdateAsync(UserUpdateModel model)
    {
        ForumException.ThrowIfNull(model);

        var existingUser = this.unitOfWork.UserRepository.GetByIdAsync(model.Id);

        var user = this.mapper.MapWithExceptionHandling<User>(model);
        this.unitOfWork.UserRepository.Update(user);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        if (this.unitOfWork.UserRepository.IsExist(modelId))
        {
            throw new ForumException("User with this Id does not exist");
        }

        await this.unitOfWork.UserRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<UserPublicProfileModel> GetUserByEmailAsync(string email)
    {
        var user = await this.unitOfWork.UserRepository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new ForumException("User with this Email does not exist");
        }

        var userProfile = this.mapper.MapWithExceptionHandling<UserPublicProfileModel>(user);

        return userProfile;
    }
}
