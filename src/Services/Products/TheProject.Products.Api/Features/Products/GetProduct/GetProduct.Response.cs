namespace TheProject.Products.Api.Features.Products.GetProduct;

public sealed record GetProductResponse(
    Guid id,
    string Sku,
    string Name,
    decimal Price,
    string Currency,
    DateTimeOffset createdAt,
    DateTimeOffset updatedAt,
    uint Version);