namespace WebApp.Infrastructure.Interfaces.IServices;

using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Models.UserModels;

public interface IUserService
{
    Task<IEnumerable<UserPublicProfileModel>> GetAllAsync(UserQueryParametersModel? queryParameters = default);

    Task<UserPublicProfileModel> GetByIdAsync(params object[] keys);

    Task<int> AddAsync(UserRegisterModel model);

    Task UpdateAsync(UserUpdateModel model);

    Task DeleteAsync(int modelId);

    public Task<string> LoginAsync(UserLoginModel model);

    public Task<int> RegisterAsync(UserRegisterModel model);

    public Task DeleteMyProfileAsync(string password, int modelId);

    public Task<UserModel> GetByIdWithDetailsAsync(int id);

    public Task<UserRole> GetUserRoleAsync(int userId);
}
