using Microsoft.EntityFrameworkCore;
using TheProject.BuildingBlocks.Results;
using TheProject.BuildingBlocks.Slices;
using TheProject.Products.Api.Domain;
using TheProject.Products.Api.Persistence;

namespace TheProject.Products.Api.Features.Products.GetProduct;

public class GetProductHandler(
    ILogger<GetProductHandler> logger,
    ProductsDbContext db
    ) : IHandler<GetProductRequest, GetProductResponse>
{

    public async Task<Result<GetProductResponse>> HandleAsync(GetProductRequest request,
        CancellationToken cancellationToken)
    {

        var rto = await db.Products
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .Select(p => new GetProductResponse(
                p.Id,
                p.Sku.Value,
                p.Name,
                p.Price.Amount,
                p.Price.Currency,
                p.CreatedAt,
                p.UpdatedAt,
                EF.Property<uint>(p, ProductsDbContext.VersionProperty)))
            .SingleOrDefaultAsync(cancellationToken);

        ProductsLogger.ProductFound(logger, request.Id);

        return rto is null ? ProductErrors.NotFound(request.Id)
                           : rto;
    }
}