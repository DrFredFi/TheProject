using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using TheProject.Products.Api.Domain;

namespace TheProject.Products.Api.Persistence;

public sealed class ProductsDbContext : DbContext
{
    public const string VersionProperty = "Version";
    public DbSet<Product> Products => Set<Product>();


    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);

    }


}