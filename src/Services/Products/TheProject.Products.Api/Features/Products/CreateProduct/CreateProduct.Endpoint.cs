using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TheProject.BuildingBlocks.Http;
using TheProject.BuildingBlocks.Results;
using TheProject.BuildingBlocks.Slices;
using TheProject.Products.Api.Features.Products.GetProduct;

namespace TheProject.Products.Api.Features.Products.CreateProduct;


public sealed record CreateProductRequest(string Sku, string Name, decimal Price, string Currency);
public sealed record CreateProductResponse(Guid Id,
    string Sku,
    string Name,
    decimal Price,
    string Currency,
    DateTimeOffset CreatedAt);



public sealed class CreateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", HandleAsync)
            .WithName("CreateProduct")
            .WithTags("Products");
    }

    private static async Task<Results<CreatedAtRoute<CreateProductResponse>, ProblemHttpResult>> HandleAsync(
        [FromBody] CreateProductRequest request,
        IHandler<CreateProductRequest, CreateProductResponse> handler,
        CancellationToken ct)
    {
        var rto = await handler.HandleAsync(request, ct);

        return rto.Match(Results<CreatedAtRoute<CreateProductResponse>, ProblemHttpResult>
            (response) => TypedResults.CreatedAtRoute(response, GetProductEndpoint.Name, new { id = response.Id }),
            (error) => error.ToProblem()
            );


    }
}