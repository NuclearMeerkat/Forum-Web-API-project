namespace WebApp.Infrastructure.Interfaces.Auth;

public interface IPasswordHasher
{
    string HashPassword(string password);

    bool Verify( string providedPassword, string hashedPassword);
}
