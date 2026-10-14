using Microsoft.EntityFrameworkCore;
using TheProject.BuildingBlocks.Results;
using TheProject.BuildingBlocks.Slices;
using TheProject.Products.Api.Domain;
using TheProject.Products.Api.Persistence;

namespace TheProject.Products.Api.Features.Products.GetProduct;

public sealed class GetProductHandler(
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
            .Select(GetProductMapping.ToResponse)
            .SingleOrDefaultAsync(cancellationToken);

        if (rto is null)
        {
            return ProductErrors.NotFound(request.Id);
        }
        else
        {
            ProductsLogger.ProductFound(logger, request.Id);
            return rto;
        }
    }
}