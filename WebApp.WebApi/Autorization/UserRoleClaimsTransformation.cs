using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Interfaces.IServices;

namespace WebApp.WebApi.Autorization;

public class UserRoleClaimsTransformation : IClaimsTransformation
{
    private readonly IUserService userService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserRoleClaimsTransformation(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        this.userService = userService;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;

        // Check if the principal is authenticated and contains the relevant claim (e.g., UserId)
        if (identity == null || !identity.IsAuthenticated)
        {
            return principal; // If not authenticated, no need to transform
        }

        // Retrieve the user ID claim (adjust claim type based on your setup)
        var userIdClaim = principal.FindFirst("userId"); // or use a custom claim type like "UserId"

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return principal; // If no valid user ID claim, do not transform
        }

        // Attempt to get the user role using the user ID
        UserRole userRole;
        try
        {
            userRole = await userService.GetUserRoleAsync(userId);
            identity.AddClaim(new Claim("Role", userRole.ToString()));
        }
        catch (ForumException)
        {
            return principal; // Return unchanged if user role retrieval fails
        }

        return principal;
    }
}
