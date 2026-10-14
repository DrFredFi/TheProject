using Microsoft.EntityFrameworkCore;
using Npgsql;
using TheProject.BuildingBlocks.Results;
using TheProject.BuildingBlocks.Slices;
using TheProject.Products.Api.Domain;
using TheProject.Products.Api.Persistence;

namespace TheProject.Products.Api.Features.Products.CreateProduct;

public sealed class CreateProductHandler(ProductsDbContext db,
    TimeProvider timeProvider,
    ILogger<CreateProductHandler> logger
    ) : IHandler<CreateProductRequest, CreateProductResponse>
{
    public async Task<Result<CreateProductResponse>> HandleAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var skuResult = Sku.Create(request.Sku);
        if (skuResult.IsFailure)
        {
            return skuResult.Error;
        }

        var priceResult = Money.Create(request.Price, request.Currency);
        if (priceResult.IsFailure)
        {
            return priceResult.Error;
        }

        var productResult = Product.Create(skuResult.Value, request.Name, priceResult.Value, timeProvider.GetUtcNow());
        if (productResult.IsFailure)
        {
            return productResult.Error;
        }

        if (await db.Products.AnyAsync(p => p.Sku == skuResult.Value, cancellationToken))
        {
            return ProductErrors.SkuTaken(skuResult.Value);
        }



        var product = productResult.Value;
        db.Products.Add(product);



        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            return ProductErrors.SkuTaken(skuResult.Value);
        }

        ProductsLogger.ProductCreated(logger, product.Id, product.Sku.Value);

        return new CreateProductResponse(
            product.Id,
            product.Sku.Value,
            product.Name,
            product.Price.Amount,
            product.Price.Currency,
            product.CreatedAt);
    }
}