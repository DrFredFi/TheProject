using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheProject.Products.Api.Domain;
using TheProject.Products.Api.Persistence;

namespace TheProject.Products.Api.Features.Products.GetProduct;

internal static class GetProductMapping
{
    public static readonly Expression<Func<Product, GetProductResponse>> ToResponse = p => new GetProductResponse(
        p.Id,
        p.Sku.Value,
        p.Name,
        p.Price.Amount,
        p.Price.Currency,
        p.CreatedAt,
        p.UpdatedAt,
        EF.Property<uint>(p, ProductsDbContext.VersionProperty)
        );

}