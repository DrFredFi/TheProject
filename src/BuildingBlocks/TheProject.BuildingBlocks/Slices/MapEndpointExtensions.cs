using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;



namespace TheProject.BuildingBlocks.Slices;

public static class MapEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapEndpoints()
        {
            var endpoints = app.ServiceProvider.GetServices<IEndpoint>();

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(app);
            }

            return app;
        }

    }
}
