using Microsoft.AspNetCore.Http.HttpResults;
using TheProject.BuildingBlocks.Http;
using TheProject.BuildingBlocks.Results;
using TheProject.BuildingBlocks.Slices;

namespace TheProject.Products.Api.Features.Products.GetProduct;

public sealed record GetProductRequest(Guid Id);
public sealed record GetProductResponse(
    Guid Id,
    string Sku,
    string Name,
    decimal Price,
    string Currency,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    uint Version);


public sealed class GetProductEndpoint : IEndpoint
{
    public const string Name = "GetProduct";
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", HandleAsync)
            .WithName(Name)
            .WithTags("Products");
    }

    private static async Task<Results<Ok<GetProductResponse>, ProblemHttpResult>> HandleAsync(
        Guid id,
        IHandler<GetProductRequest, GetProductResponse> handler,
        CancellationToken ct
        )
    {
        var rto = await handler.HandleAsync(new GetProductRequest(id), ct);

        return rto.Match(
            Results<Ok<GetProductResponse>, ProblemHttpResult> (response) => TypedResults.Ok(response),
            error => error.ToProblem()
            );


    }
}