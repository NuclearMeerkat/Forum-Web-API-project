using WebApp.Infrastructure.Entities;

namespace WebApp.Infrastructure.Interfaces.Auth;

public interface IJwtProvider
{
    public string GenerateToken(User user);
}
