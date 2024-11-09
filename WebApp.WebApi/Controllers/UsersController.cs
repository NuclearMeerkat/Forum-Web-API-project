using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.TopicModels;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : BaseController
{
    private readonly IUserService userService;
    private readonly IServiceProvider serviceProvider;

    public UsersController(IUserService userService, IServiceProvider serviceProvider)
    {
        this.userService = userService;
        this.serviceProvider = serviceProvider;
    }

    // GET: api/users
    // Restricted to admins only
    [HttpGet]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] TopicQueryParametersModel parameters)
    {
        var users = await this.userService.GetAllAsync(parameters);
        return this.Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        UserModel user;
        try
        {
            user = await this.userService.GetByIdAsync(id);
        }
        catch (ForumException)
        {
            return this.NotFound();
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
                return this.CreatedAtAction(nameof(this.GetById), new { id = id }, registerDto);
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
                    return Unauthorized("Invalid username or password.");
                }

                return Ok(token);
            }
            catch (ForumException e)
            {
                return Unauthorized(e.Message);
            }
        });
    }

    // PUT: api/users/{id}
    [HttpPut("{id:int}")]
    //[Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateModel updateDto)
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
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id:int}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
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
