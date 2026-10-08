using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using TheProject.BuildingBlocks.Results;
using TheProject.BuildingBlocks.Slices;

namespace TheProject.BuildingBlocks.Tests.Unit.Slices;

public class SliceRegistrationExtensionsTests
{
    public sealed record Ping(string Text);

    public sealed class PingHandler : IHandler<Ping, string>
    {
        public Task<Result<string>> HandleAsync(Ping request, CancellationToken cancellationToken) =>
            Task.FromResult<Result<string>>(request.Text);
    }

    public sealed class OtherPingHandler : IHandler<Ping, string>
    {
        public Task<Result<string>> HandleAsync(Ping request, CancellationToken cancellationToken) =>
            Task.FromResult<Result<string>>("other");
    }

    public sealed class ForgetPingHandler : IHandler<Ping>
    {
        public Task<Result> HandleAsync(Ping request, CancellationToken cancellationToken) =>
            Task.FromResult(Result.Success());
    }

    public abstract class AbstractPingHandler : IHandler<Ping, string>
    {
        public abstract Task<Result<string>> HandleAsync(Ping request, CancellationToken cancellationToken);
    }

    public sealed class GenericPingHandler<T> : IHandler<T, string>
    {
        public Task<Result<string>> HandleAsync(T request, CancellationToken cancellationToken) =>
            Task.FromResult<Result<string>>("generic");
    }

    public sealed class PingEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) { }
    }

    private readonly ServiceCollection _services = new();

    [Fact]
    public void Handlers_resolve_by_their_closed_contract()
    {
        _services.AddSlices([typeof(PingHandler), typeof(ForgetPingHandler)]);

        using var provider = _services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        scope.ServiceProvider.GetRequiredService<IHandler<Ping, string>>().Should().BeOfType<PingHandler>();
        scope.ServiceProvider.GetRequiredService<IHandler<Ping>>().Should().BeOfType<ForgetPingHandler>();
    }

    [Fact]
    public void Two_handlers_for_the_same_contract_are_rejected()
    {
        var register = () => _services.AddSlices([typeof(PingHandler), typeof(OtherPingHandler)]);

        register.Should().Throw<InvalidOperationException>()
            .WithMessage($"*{nameof(PingHandler)}*{nameof(OtherPingHandler)}*");
    }

    [Fact]
    public void Scanning_twice_is_harmless()
    {
        Type[] types = [typeof(PingHandler), typeof(PingEndpoint)];

        _services.AddSlices(types).AddSlices(types);

        _services.Where(d => d.ServiceType == typeof(IEndpoint)).Should().ContainSingle();
        _services.Where(d => d.ServiceType == typeof(IHandler<Ping, string>)).Should().ContainSingle();
    }

    [Fact]
    public void Abstract_and_open_generic_types_are_ignored()
    {
        _services.AddSlices([typeof(AbstractPingHandler), typeof(GenericPingHandler<>)]);

        _services.Should().BeEmpty();
    }
}