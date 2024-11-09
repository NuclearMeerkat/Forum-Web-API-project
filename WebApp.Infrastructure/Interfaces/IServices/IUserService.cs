using WebApp.Infrastructure.Models.TopicModels;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.Infrastructure.Interfaces.IServices;
public interface IUserService : ICrud<UserModel, UserRegisterModel, UserUpdateModel, TopicQueryParametersModel, int>
{
    public Task<string> LoginAsync(UserLoginModel model);

    public Task<int> RegisterAsync(UserRegisterModel model);

}
