using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.WebApi.Controllers;

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

    // GET: api/users
    // Restricted to admins only
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

    [HttpGet("details/{id:int}")]
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

    // GET: api/users/{id}
    [HttpGet("{id:int}")]
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

    // POST: api/users/register
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

    // POST: api/users/login
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
            catch (InvalidOperationException e)
            {
                return this.Unauthorized("Invalid username or password.");
            }
            catch (ForumException e)
            {
                return this.Unauthorized("Invalid username or password");
            }
        });
    }

    // PUT: api/users/{id}
    [HttpPatch("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UserUpdateModel updateDto)
    {
        var validator = this.serviceProvider.GetService<IValidator<UserUpdateModel>>();

        updateDto.Id = this.GetCurrentUserId(httpContextAccessor);

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

    // DELETE: api/users/{id}
    [HttpDelete("profile")]
    [Authorize]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMyProfile(string password)
    {
        int userId = this.GetCurrentUserId(httpContextAccessor);
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

    // PUT: api/users/{id}
    [HttpPatch("{id:int}")]
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

    // DELETE: api/users/{id}
    [HttpDelete("{id:int}")]
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
