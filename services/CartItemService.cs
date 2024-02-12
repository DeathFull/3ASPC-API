using _3ASPC_API.database;
using _3ASPC_API.models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace _3ASPC_API.services;

public class CartItemService
{
    private readonly ApiContext _context;

    public CartItemService(ApiContext context)
    {
        _context = context;
    }

    public async Task<List<CartItem>> GetUserCart(int userId)
    {
        return await _context.CartItems
            .Where(cc => cc.Cart.UserId == userId)
            .Select(cc => new CartItem
            {
                Id = cc.Id,
                CartId = cc.CartId,
                ProductId = cc.ProductId,
                Quantity = cc.Quantity
            })
            .ToListAsync();
    }

    public async Task<CartItem> AddProduct(int cartId, CartItemRequest cartItem)
    {
        var existingContent = await _context.CartItems
            .FirstOrDefaultAsync(cc => cc.CartId == cartId && cc.ProductId == cartItem.ProductId);

        if (existingContent != null)
        {
            throw new Exception("Already in your cart.");
        }

        var newCartItem = new CartItem
        {
            CartId = cartId,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity,
        };

        _context.CartItems.Add(newCartItem);
        await _context.SaveChangesAsync();

        return newCartItem;
    }

    public async Task<CartItem> UpdateCartItem(int cartId, CartItem cartItem)
    {
        var existProduct = await _context.CartItems
            .FirstOrDefaultAsync(cc => cc.CartId == cartId && cc.ProductId == cartItem.ProductId);

        if (existProduct == null)
        {
            throw new Exception("Product not found.");
        }

        existProduct.Quantity = cartItem.Quantity;
        await _context.SaveChangesAsync();

        return existProduct;
    }

    public async Task<bool> RemoveProductFromCart(int cartId, int productId)
    {
        var cartItem = _context.CartItems
            .Where(cc => cc.ProductId == productId && cc.CartId == cartId);

        _context.CartItems.RemoveRange(cartItem);
        await _context.SaveChangesAsync();
        return true;
    }
}