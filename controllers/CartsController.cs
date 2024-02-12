using _3ASPC_API.models;
using _3ASPC_API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3ASPC_API.controllers;

[ApiController]
[Route("carts")]
public class CartsController (CartService cartService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Cart>> GetCartByUserId()
    {
        var userId = User.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int idToken))
        {
            return BadRequest("Invalid user Id.");
        }

        var userCart = await cartService.GetCartById(idToken);

        if (userCart == null)
        {
            return NotFound("User cart not found.");
        }
        Console.WriteLine(userCart);
        return Ok(userCart);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Cart>> CreateCartUser()
    {
        try
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int idToken))
            {
                return BadRequest("Invalid user Id.");
            }

            var createCart = await cartService.CreateCart(idToken);

            if (createCart == null)
            {
                return NotFound("Cart creation failed.");
            }

            return Ok(createCart);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}