using _3ASPC_API.models;
using Microsoft.EntityFrameworkCore;

namespace _3ASPC_API.database;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
    }

    public DbSet<ProductItem> ProductItems { get; set; } = null!;
}
