using _3ASPC_API.models;
using _3ASPC_API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3ASPC_API.controllers;

[ApiController]
[Route("cartItems")]
public class CartItemsController(CartItemService cartItemService, CartService cartService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CartItem>> GetUserCart()
    {
        var id = User.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idToken))
        {
            return BadRequest("Invalid user Id.");
        }

        var userCart = await cartItemService.GetUserCart(idToken);

        if (userCart == null || userCart.Count == 0)
        {
            return NotFound("User cart not found.");
        }

        return Ok(userCart);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CartItem>> AddProductInCart([FromBody] CartItemRequest cartItem)
    {
        try
        {
            var id = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idToken))
            {
                return BadRequest("Invalid user Id.");
            }

            var cart = await cartService.GetCartById(idToken);
            var addProduct = await cartItemService.AddProduct(cart.Id, cartItem);
            if (addProduct == null)
            {
                return NotFound("User not found.");
            }

            return Ok(addProduct);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<CartItem>> UpdateCartItem([FromBody] CartItemRequest cartItem)
    {
        var id = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idToken))
        {
            return BadRequest("Invalid user Id.");
        }

        var cart = await cartService.GetCartById(idToken);
        var updateAction = await cartItemService.UpdateCartItem(cart.Id, new CartItem
        {
            CartId = cart.Id,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity
        });
        if (updateAction == null)
        {
            return NotFound("Product not found.");
        }

        return Ok(updateAction);
    }

    [Authorize]
    [HttpDelete("{productId}")]
    public async Task<ActionResult<bool>> DeleteCartProductById(int productId)
    {
        var id = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idToken))
        {
            return BadRequest("Invalid user Id.");
        }

        var cart = await cartService.GetCartById(idToken);
        var deleteAction = await cartItemService.RemoveProductFromCart(cart.Id, productId);
        if (deleteAction == false)
        {
            return NotFound("Product not found.");
        }

        return Ok(deleteAction);
    }
}