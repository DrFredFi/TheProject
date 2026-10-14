namespace TheProject.Products.Api.Features.Products.CreateProduct;

public sealed record CreateProductResponse(Guid id,
    string sku,
    string Name,
    decimal Price,
    string Currency,
    DateTimeOffset CreatedAt);