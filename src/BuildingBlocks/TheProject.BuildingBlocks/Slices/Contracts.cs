using Microsoft.AspNetCore.Routing;

using TheProject.BuildingBlocks.Results;

namespace TheProject.BuildingBlocks.Slices;


public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}

public interface IHandler<TRequest>
{
    Task<Result> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public interface IHandler<TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
