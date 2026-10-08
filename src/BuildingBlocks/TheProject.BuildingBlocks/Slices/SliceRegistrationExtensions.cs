using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;



namespace TheProject.BuildingBlocks.Slices;

public static class SliceRegistrationExtensions
{

    private static readonly Type[] HandlerContracts = [typeof(IHandler<>), typeof(IHandler<,>)];


    extension(IServiceCollection services)
    {

        public IServiceCollection AddSlices(Assembly assembly) => services.AddSlices(assembly.GetTypes());

        internal IServiceCollection AddSlices(IEnumerable<Type> candidates)
        {
            foreach (var candidate in candidates.Where(c => IsConcreteClass(c)))
            {
                //ENDPOINTS
                if (candidate.IsAssignableTo(typeof(IEndpoint)))
                {
                    services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IEndpoint), candidate));
                }


                //HANDLERS
                foreach (var handlerInterface in candidate.GetInterfaces().Where(IsHandlerContract))
                {
                    var existing = services.FirstOrDefault(t => t.ServiceType == handlerInterface);
                    //ALREADY IMPLEMENTED THAT INTERFACE WITH THE CURRENT MAIN-LOOP DESCRIPTOR
                    if (existing?.ImplementationType == candidate)
                    {
                        continue;
                    }
                    //IF THERE'S ALREADY A IMPLEMENTATION REGISTERED, AND THIS IS A DIFFERENT ONE
                    if (existing is not null)
                    {
                        throw new InvalidOperationException($"{handlerInterface} is implemented by both {existing.ImplementationType} and {candidate}." +
                            $"Each request type must have exactly one handler.");
                    }

                    services.AddScoped(handlerInterface, candidate);
                }

            }
            return services;
        }

    }

    private static bool IsConcreteClass(Type type) =>
        type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false };

    private static bool IsHandlerContract(Type type) =>
        type.IsGenericType && HandlerContracts.Contains(type.GetGenericTypeDefinition());
}