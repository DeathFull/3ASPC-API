using _3ASPC_API.database;
using _3ASPC_API.models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace _3ASPC_API.services;

public class ProductService
{
    private readonly ApiContext _context;

    public ProductService(ApiContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<List<Product>> GetProductsBySellerId(int id)
    {
        return await _context.Products.Where(p => p.SellerId == id).ToListAsync();
    }

    public async Task<List<Product>> SearchProduct(string searchQuery)
    {
        searchQuery = searchQuery.ToLower();
        return await _context.Products
            .Where(p =>
                        p.Name.ToLower().Contains(searchQuery) ||
                         p.SellerUser.Pseudo.ToLower().Contains(searchQuery) ||
                         p.SellerUser.Email.ToLower().Contains(searchQuery) ||
                         p.Id.ToString().Contains(searchQuery))
            .Include(p => p.SellerUser)
            .ToListAsync();
    }

    public async Task<Product> GetProductById(int productId)
    {
        return await _context.Products
            .Include(p => p.SellerUser)
            .FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<Product> CreateProduct(int sellerId, ProductRequest product)
    {
        var sellerUser = await _context.Users.FindAsync(sellerId);
        if (sellerUser == null)
        {
            throw new Exception("Seller not found.");
        }

        var newProduct = new Product
        {
            Name = product.Name,
            Image = product.Image,
            Price = product.Price,
            Available = product.Available,
            AddedTime = product.AddedTime,
            SellerId = sellerId,
            SellerUser = sellerUser
        };

        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();

        return newProduct;
    }

    public async Task<Product> UpdateProductById(int productId, int sellerId, Product product)
    {
        var existProduct = await _context.Products.FirstOrDefaultAsync(u => u.Id == productId);
        if (existProduct == null)
        {
            throw new Exception("Product not found.");
        }

        if (existProduct.SellerId != sellerId)
        {
            throw new Exception("Product isn't yours.");
        }

        existProduct.Price = product.Price;
        existProduct.Available = product.Available;
        existProduct.Price = product.Price;

        await _context.SaveChangesAsync();
        return existProduct;
    }

    public async Task<bool> DeleteProductsById(int productId, int sellerId)
    {
        var product = await _context.Products
            .Include(products => products.SellerUser)
            .SingleOrDefaultAsync(p => p.Id == productId && p.SellerId == sellerId);
        if (product == null)
        {
            throw new Exception("Product not found.");
        }

        if (product.SellerId != sellerId)
        {
            throw new Exception("Product isn't yours.");
        }

        await RemoveProductFromCart(productId);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<List<int>> GetCartIdByProductId(int productId)
    {
        return await _context.CartItems
            .Where(ci => ci.ProductId == productId)
            .Select(ci => ci.Id)
            .ToListAsync();
    }

    private async Task<bool> RemoveProductFromCart(int productId)
    {
        var cartItemsIds = await GetCartIdByProductId(productId);
        if (cartItemsIds.Any())
        {
            _context.CartItems.RemoveRange(cartItemsIds.Select(id => new CartItem { Id = id }));
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}