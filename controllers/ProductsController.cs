using _3ASPC_API.models;
using _3ASPC_API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3ASPC_API.controllers;

[ApiController]
[Route("products")]
public class ProductsController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await productService.GetProducts();
        if (products == null || products.Count == 0)
        {
            return NotFound("Product not found.");
        }
        return Ok(products);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Product>>> SearchProducts([FromQuery] string search)
    {
        var products = await productService.SearchProduct(search);
        if (products == null || products.Count == 0)
        {
            return NotFound("Product not found.");
        }
        return Ok(products);
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<Product>> GetProductById(int productId)
    {
        var product = await productService.GetProductById(productId);

        if (product == null)
        {
            return NotFound("No product found.");
        }

        return Ok(product);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductRequest product)
    {
        try
        {
            var id = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idToken))
            {
                return BadRequest("Invalid seller ID");
            }
            var createdProduct = await productService.CreateProduct(idToken, product);
            if (createdProduct == null)
            {
                return NotFound("Product creation failed.");
            }
            return Ok(createdProduct);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize]
    [HttpPut("{productId}")]
    public async Task<ActionResult<Product>> UpdateProduct(int productId, [FromBody] Product product)
    {
        var id = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idToken))
        {
            return BadRequest("Invalid user ID");
        }
        var updatedProduct = await productService.UpdateProductById(productId, idToken, product);
        if (updatedProduct == null)
        {
            return NotFound("Update product failed.");
        }
        return Ok(updatedProduct);
    }

    [Authorize]
    [HttpDelete("{productId}")]
    public async Task<ActionResult<bool>> DeleteProductById(int productId)
    {
        try
        {
            var id = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idToken))
            {
                return BadRequest("Invalid seller Id.");
            }

            var isDeleted = await productService.DeleteProductsById(productId, idToken);
            return Ok(isDeleted);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

    }
}