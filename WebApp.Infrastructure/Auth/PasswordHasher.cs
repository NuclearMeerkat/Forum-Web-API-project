using WebApp.Infrastructure.Interfaces.Auth;

namespace WebApp.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool Verify(string providedPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword);
    }
}
