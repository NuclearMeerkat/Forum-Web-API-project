namespace WebApp.Infrastructure.Auth;

using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.Auth;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        this.options = options.Value;
    }

    public string GenerateToken(User user)
    {
        Claim[] claims =
            [new("userId",
            user.Id.ToString(CultureInfo.InvariantCulture))];

        var signingCredentials =
            new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(this.options.ExpiresHours));

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}
