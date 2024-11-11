namespace WebApp.Infrastructure.Interfaces.IServices;

using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Models.UserModels;

public interface IUserService : ICrud<UserPublicProfileModel, UserRegisterModel, UserUpdateModel, UserQueryParametersModel, int>
{
    public Task<string> LoginAsync(UserLoginModel model);

    public Task<int> RegisterAsync(UserRegisterModel model);

    public Task DeleteMyProfileAsync(string password, int modelId);

    public Task<UserModel> GetByIdWithDetailsAsync(int id);

    public Task<UserRole> GetUserRoleAsync(int userId);

}
