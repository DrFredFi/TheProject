namespace TheProject.Products.Api.Features.Products.CreateProduct;

public sealed record CreateProductRequest(string Sku, string Name, decimal Price, string Currency);