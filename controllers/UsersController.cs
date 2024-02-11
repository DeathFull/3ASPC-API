using _3ASPC_API.models;
using _3ASPC_API.services;
using Microsoft.AspNetCore.Mvc;

namespace _3ASPC_API.controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserRequest user)
    {
        try
        {
            var newUser = await userService.RegisterAsync(new User
            {
                Email = user.Email,
                Pseudo = user.Pseudo,
                Password = user.Password,
                Role = user.Role
            });
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<User>> Authenticate(User user)
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