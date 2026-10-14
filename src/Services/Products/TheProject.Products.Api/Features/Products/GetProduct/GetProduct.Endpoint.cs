using Microsoft.AspNetCore.Http.HttpResults;
using TheProject.BuildingBlocks.Http;
using TheProject.BuildingBlocks.Results;
using TheProject.BuildingBlocks.Slices;

namespace TheProject.Products.Api.Features.Products.GetProduct;

public class GetProductEndpoint : IEndpoint
{
    public const string Name = "GetProduct";
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", HandleAsync)
            .WithName(Name)
            .WithTags("Products");
    }

    private async Task<Results<Ok<GetProductResponse>, ProblemHttpResult>> HandleAsync(
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