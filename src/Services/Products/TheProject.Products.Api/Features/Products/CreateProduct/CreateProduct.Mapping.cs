using TheProject.Products.Api.Domain;

namespace TheProject.Products.Api.Features.Products.CreateProduct;

internal static class CreateProductMapping
{
    public static CreateProductResponse ToResponse(Product product) => new(
        product.Id, product.Sku.Value, product.Name,
        product.Price.Amount, product.Price.Currency,
        product.CreatedAt);
}