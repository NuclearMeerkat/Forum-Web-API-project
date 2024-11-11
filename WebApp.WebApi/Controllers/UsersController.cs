using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.WebApi.Controllers;

/// <summary>
/// Controller for managing user accounts and profiles.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : BaseController
{
    private readonly IUserService userService;
    private readonly IServiceProvider serviceProvider;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UsersController(
        IUserService userService,
        IServiceProvider serviceProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        this.userService = userService;
        this.serviceProvider = serviceProvider;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets a list of all user profiles.
    /// Restricted to administrators only.
    /// </summary>
    /// <param name="parameters">Query parameters for filtering, sorting and pagination.</param>
    /// <returns>A list of user profiles.</returns>
    [HttpGet]

    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsersProfiles([FromQuery] UserQueryParametersModel parameters)
    {
        var validator = this.serviceProvider.GetService<IValidator<UserQueryParametersModel>>();

        return await this.ValidateAndExecuteAsync(parameters, validator, async () =>
        {
            var users = await this.userService.GetAllAsync(parameters);
            return this.Ok(users);
        });
    }

    /// <summary>
    /// Gets detailed information about a specific user by their ID.
    /// Requires authorization.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>Detailed user information if found; otherwise, NotFound.</returns>
    [HttpGet("{id:int}/details")]
    [Authorize]
    public async Task<IActionResult> GetUserDetailedInfoById(int id)
    {
        UserModel user;
        try
        {
            user = await this.userService.GetByIdWithDetailsAsync(id);
        }
        catch (ForumException ex)
        {
            return this.NotFound(ex.Message);
        }

        return this.Ok(user);
    }

    /// <summary>
    /// Gets public profile information of a user by their ID.
    /// Requires authorization.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>Public profile information if found; otherwise, NotFound.</returns>
    [HttpGet("{id:int}/profile")]
    [Authorize]
    public async Task<IActionResult> GetUserProfileById(int id)
    {
        UserPublicProfileModel user;
        try
        {
            user = await this.userService.GetByIdAsync(id);
        }
        catch (ForumException ex)
        {
            return this.NotFound(ex.Message);
        }

        return this.Ok(user);
    }

    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <param name="registerDto">The registration details of the user.</param>
    /// <returns>Created response with user ID, nickname, and email if successful; otherwise, BadRequest.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel registerDto)
    {
        var validator = this.serviceProvider.GetService<IValidator<UserRegisterModel>>();

        return await this.ValidateAndExecuteAsync(registerDto, validator, async () =>
        {
            try
            {
                ArgumentNullException.ThrowIfNull(registerDto);
                int id = await this.userService.RegisterAsync(registerDto);
                return this.CreatedAtAction(
                    nameof(this.Register),
                    new { id = id, nickname = registerDto.Nickname, email = registerDto.Email });
            }
            catch (ForumException e)
            {
                return this.BadRequest(e.Message);
            }
        });
    }

    /// <summary>
    /// Authenticates a user and issues a token on successful login.
    /// </summary>
    /// <param name="loginDto">The login details of the user.</param>
    /// <returns>Ok if login is successful and token is issued; otherwise, Unauthorized.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel loginDto)
    {
        var validator = this.serviceProvider.GetService<IValidator<UserLoginModel>>();

        return await this.ValidateAndExecuteAsync(loginDto, validator, async () =>
        {
            try
            {
                var token = await this.userService.LoginAsync(loginDto);
                if (string.IsNullOrEmpty(token))
                {
                    return this.Unauthorized("Invalid username or password.");
                }

                this.httpContextAccessor.HttpContext?.Response.Cookies.Append("fancy-cookies", token);

                return this.Ok();
            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized("Invalid username or password.");
            }
            catch (ForumException)
            {
                return this.Unauthorized("Invalid username or password");
            }
        });
    }

    /// <summary>
    /// Updates the profile of the currently logged-in user.
    /// </summary>
    /// <param name="updateDto">The updated profile details.</param>
    /// <returns>NoContent if the update is successful; otherwise, BadRequest or NotFound.</returns>
    [HttpPatch("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UserUpdateModel updateDto)
    {
        var validator = this.serviceProvider.GetService<IValidator<UserUpdateModel>>();

        updateDto.Id = this.GetCurrentUserId(this.httpContextAccessor);

        return await this.ValidateAndExecuteAsync(updateDto, validator, async () =>
        {
            try
            {
                await this.userService.UpdateAsync(updateDto);
                return this.NoContent();
            }
            catch (ForumException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                return this.NotFound("User was not found");
            }
        });
    }

    /// <summary>
    /// Deletes the profile of the currently logged-in user.
    /// Requires password confirmation.
    /// </summary>
    /// <param name="password">The user's password for confirmation.</param>
    /// <returns>NoContent if successful; otherwise, BadRequest.</returns>
    [HttpDelete("profile")]
    [Authorize]

    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMyProfile(string password)
    {
        int userId = this.GetCurrentUserId(this.httpContextAccessor);
        try
        {
            await this.userService.DeleteMyProfileAsync(password, userId);
            return this.NoContent();
        }
        catch (ForumException e)
        {
            return this.BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Updates a user's profile by ID as an administrator.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="updateDto">The updated profile details.</param>
    /// <returns>NoContent if successful; otherwise, BadRequest or NotFound.</returns>
    [HttpPatch("admin/{id:int}")]

    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminUpdateUser(int id, [FromBody] UserUpdateModel updateDto)
    {
        try
        {
            updateDto.Id = id;
            await this.userService.UpdateAsync(updateDto);
            return this.NoContent();
        }
        catch (ForumException e)
        {
            return this.BadRequest(e.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            return this.NotFound("User was not found");
        }
    }

    /// <summary>
    /// Deletes a user by ID as an administrator.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>NoContent if successful; otherwise, BadRequest.</returns>
    [HttpDelete("admin/{id:int}")]

    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminDelete(int id)
    {
        try
        {
            await this.userService.DeleteAsync(id);
            return this.NoContent();
        }
        catch (ForumException e)
        {
            return this.BadRequest(e.Message);
        }
    }
}
