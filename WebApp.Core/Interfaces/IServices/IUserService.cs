using WebApp.Core.Models.TopicModels;
using WebApp.Core.Models.UserModels;

namespace WebApp.Core.Interfaces.IServices;
public interface IUserService : ICrud<UserModel, UserCreateModel, UserUpdateModel, TopicQueryParametersModel, int>
{
}
