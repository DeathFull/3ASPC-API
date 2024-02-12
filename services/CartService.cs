using _3ASPC_API.database;
using _3ASPC_API.models;
using Microsoft.EntityFrameworkCore;

namespace _3ASPC_API.services;

public class CartService(ApiContext context)
{
    public async Task<Cart> GetCartById(int id)
    {
        return await context.Carts.FirstOrDefaultAsync(c => c.UserId == id);
    }

    public async Task<Cart> CreateCart(int id)
    {
        var newCart = new Cart
        {
            UserId = id,
        };
        context.Carts.Add(newCart);
        await context.SaveChangesAsync();
        return newCart;
    }
}