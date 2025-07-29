using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Arahk.MyMediatr;

public interface IRequestHandler<TRequest, TResult>
{
    Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);
}

public static class MyMediatrExtensions
{
    public static IServiceCollection AddMyMediatr(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<MyMediatr>(config =>
        {
            return new MyMediatr(config.GetRequiredService<IServiceProvider>())
            {
                Assembly = assembly
            };
        });

        // Automatically register all handlers from the assembly
        RegisterHandlersFromAssembly(services, assembly);

        return services;
    }

    private static void RegisterHandlersFromAssembly(IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract)
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            foreach (var interfaceType in interfaces)
            {
                services.AddScoped(interfaceType, handlerType);
            }
        }
    }
}

public class MyMediatr(IServiceProvider serviceProvider)
{
    internal Assembly? Assembly { get; set; }

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public Task<TResult> ExecuteAsync<TRequest, TResult>(TRequest request)
        where TRequest : class
    {
        var handler = _serviceProvider.GetService<IRequestHandler<TRequest, TResult>>() ?? throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");

        return handler.Handle(request, CancellationToken.None);
    }
}
