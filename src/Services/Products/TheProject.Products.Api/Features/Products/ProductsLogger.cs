namespace TheProject.Products.Api.Features.Products;

internal static partial class ProductsLogger
{
    [LoggerMessage(level: LogLevel.Information, Message = "Product {ProductId} created with SKU {Sku}")]
    public static partial void ProductCreated(ILogger logger, Guid productId, string sku);

    [LoggerMessage(level: LogLevel.Information, Message = "Product {ProductId} found")]
    public static partial void ProductFound(ILogger logger, Guid productId);

}