using AutoMapper;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Interfaces.Auth;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IPasswordHasher passwordHasher;
    private readonly IJwtProvider jwtProvider;

    public UserService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        this.jwtProvider = jwtProvider;
    }

    public async Task<IEnumerable<UserPublicProfileModel>> GetAllAsync(
        UserQueryParametersModel? queryParameters = default)
    {
        if (queryParameters == null)
        {
            var users = await this.unitOfWork.UserRepository.GetAllAsync();
            var userModels = users.Select(u => this.mapper.MapWithExceptionHandling<UserPublicProfileModel>(u));
            return userModels;
        }

        var userEntities = await this.unitOfWork.UserRepository.GetRange(
            (queryParameters.Page - 1) * queryParameters.Size,
            queryParameters.Size);

        var query = userEntities.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Search))
        {
            query = query.Where(u => u.Nickname.Contains(queryParameters.Search) ||
                                     u.Email.Contains(queryParameters.Search));
        }

        query = queryParameters.SortBy switch
        {
            "Nickname" => queryParameters.Ascending
                ? query.OrderBy(t => t.Nickname)
                : query.OrderByDescending(t => t.Nickname),
            "CreatedDate" => queryParameters.Ascending
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt),
            _ => queryParameters.Ascending ? query.OrderBy(t => t.Nickname) : query.OrderByDescending(t => t.Nickname)
        };

        var queryModels = query.Select(u => this.mapper.MapWithExceptionHandling<UserPublicProfileModel>(u));

        return queryModels;
    }

    public async Task<UserPublicProfileModel> GetByIdAsync(params object[] keys)
    {
        if (!this.unitOfWork.UserRepository.IsExist(keys))
        {
            throw new ForumException("User with this Id does not exist");
        }

        var userEntity = await this.unitOfWork.UserRepository.GetByIdAsync(keys);
        var userModel = this.mapper.MapWithExceptionHandling<UserPublicProfileModel>(userEntity);

        return userModel;
    }

    public async Task<UserModel> GetByIdWithDetailsAsync(int id)
    {
        if (!this.unitOfWork.UserRepository.IsExist(id))
        {
            throw new ForumException("User with this Id does not exist");
        }

        var userEntity = await this.unitOfWork.UserRepository.GetWithDetailsAsync(id);
        var userModel = this.mapper.MapWithExceptionHandling<UserModel>(userEntity);

        return userModel;
    }

    public async Task<int> RegisterAsync(UserRegisterModel model)
    {
        try
        {
            ForumException.ThrowIfNull(model);

            model.Password = this.passwordHasher.HashPassword(model.Password);

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

    public async Task<int> AddAsync(UserRegisterModel model)
    {
        try
        {
            ForumException.ThrowIfNull(model);

            model.Password = this.passwordHasher.HashPassword(model.Password);

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
        User user;
        try
        {
            user = await this.unitOfWork.UserRepository.GetByEmailAsync(model.Email);
        }
        catch (Exception)
        {
            throw new UnauthorizedAccessException("Invalid password or email");
        }

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid password or email");
        }

        var result = this.passwordHasher.Verify(model.Password, user.PasswordHash);

        if (!result)
        {
            throw new UnauthorizedAccessException("Invalid password or email");
        }

        user.LastLogin = DateTime.UtcNow;

        this.unitOfWork.UserRepository.Update(user);

        var token = this.jwtProvider.GenerateToken(user);

        return token;
    }

    public async Task UpdateAsync(UserUpdateModel model)
    {
        ForumException.ThrowIfNull(model);

        var existingUser = await this.unitOfWork.UserRepository.GetByIdAsync(model.Id);

        if (existingUser.Id != model.Id)
        {
            throw new UnauthorizedAccessException("User with this id can not update this profile");
        }

        var user = this.mapper.Map(model, existingUser);
        this.unitOfWork.UserRepository.Update(user);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        if (!this.unitOfWork.UserRepository.IsExist(modelId))
        {
            throw new ForumException("User with this Id does not exist");
        }

        await this.unitOfWork.UserRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteMyProfileAsync(string password, int modelId)
    {
        if (!this.unitOfWork.UserRepository.IsExist(modelId))
        {
            throw new ForumException("User with this Id does not exist");
        }

        var user = await this.unitOfWork.UserRepository.GetWithDetailsAsync(modelId);

        var result = this.passwordHasher.Verify(password, user.PasswordHash);

        if (!result)
        {
            throw new ForumException("Invalid password");
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

    public async Task<UserRole> GetUserRoleAsync(int userId)
    {
        if (!this.unitOfWork.UserRepository.IsExist(userId))
        {
            throw new ForumException("User with this Id does not exist");
        }

        var user = await this.unitOfWork.UserRepository.GetByIdAsync(userId);
        return user.Role;
    }
}
