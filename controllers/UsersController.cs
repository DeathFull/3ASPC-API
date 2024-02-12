using _3ASPC_API.models;
using _3ASPC_API.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3ASPC_API.controllers;

[ApiController]
[Route("users")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserRequest user)
    {
        try
        {
            var newUser = await userService.RegisterAsync(user);
            return Ok(newUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<User>> Authenticate(UserLogin user)
    {
        try
        {
            var authenticatedUser = await userService.AuthenticateAsync(user.Email, user.Password);
            return Ok(authenticatedUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("update")]
    public async Task<ActionResult<User>> UpdateUser(UserRequest user)
    {
        try
        {
            var id = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idToken))
            {
                return Unauthorized("Invalid token.");
            }
            var updatedUser = await userService.UpdateUserAsync(idToken, user);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteUser()
    {
        try
        {
            var id = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idToken))
            {
                return BadRequest("Invalid token.");
            }

            var isDeleted = await userService.DeleteUserAsync(idToken);
            return Ok(isDeleted);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await userService.GetUserAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}