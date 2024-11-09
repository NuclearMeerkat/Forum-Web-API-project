using WebApp.Infrastructure.Entities;

namespace WebApp.Infrastructure.Interfaces.Auth;

public interface IJvtProvider
{
    public string GenerateToken(User user);
}
