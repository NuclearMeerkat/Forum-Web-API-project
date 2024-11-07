using WebApp.Core.Models;

namespace WebApp.Core.Interfaces.IServices;
public interface IUserService : ICrud<UserModel, UserCreateModel, int>
{
}
