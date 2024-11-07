using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models;

namespace WebApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    // GET: api/users
    // Restricted to admins only
    [HttpGet]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await this.userService.GetAllAsync();
        return Ok(users);
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
            return NotFound();
        }

        return Ok(user);
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateModel registerDto)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(registerDto);
            await this.userService.AddAsync(registerDto);
            return CreatedAtAction(nameof(this.GetById), new { id = registerDto.Id }, registerDto);
        }
        catch (ForumException e)
        {
            return BadRequest(e.Message);
        }
    }

    //// POST: api/users/login
    //[HttpPost("login")]
    //public async Task<IActionResult> Login([FromBody] UserLoginDtoModel loginDto)
    //{
    //    try
    //    {
    //        var token = await this.userService.AuthenticateAsync(loginDto);
    //        if (string.IsNullOrEmpty(token))
    //        {
    //            return Unauthorized("Invalid username or password.");
    //        }
    //        return Ok(new { Token = token });
    //    }
    //    catch (ForumException e)
    //    {
    //        return Unauthorized(e.Message);
    //    }
    //}

    // PUT: api/users/{id}
    [HttpPut("{id:int}")]
    //[Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UserCreateModel updateDto)
    {
        try
        {
            updateDto.Id = id;
            await this.userService.UpdateAsync(updateDto);
            return NoContent();
        }
        catch (ForumException e)
        {
            return BadRequest(e.Message);
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
            return NoContent();
        }
        catch (ForumException e)
        {
            return BadRequest(e.Message);
        }
    }
}
