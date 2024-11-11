namespace WebApp.Infrastructure.Interfaces.Auth;

using WebApp.Infrastructure.Entities;

public interface IJwtProvider
{
    public string GenerateToken(User user);
}
