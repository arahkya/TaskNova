using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Arahk.MyMediatr;

public interface IRequestValidator<TRequest>
{
    Task<bool> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}

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
        RegisterValidatorsFromAssembly(services, assembly);

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

    private static void RegisterValidatorsFromAssembly(IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract)
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestValidator<>)))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestValidator<>));

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

    public async Task<TResult> ExecuteWithValidateAsync<TRequest, TResult>(TRequest request)
        where TRequest : class
    {
        var validationContext = new ValidationContext(request);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

        var customValidator = _serviceProvider.GetService<IRequestValidator<TRequest>>();
        var isCustomValidateValid = await (customValidator?.ValidateAsync(request, CancellationToken.None) ?? Task.FromResult(false));

        if (!isValid || !isCustomValidateValid)
        {
            throw new InvalidOperationException($"Validation failed for request of type {typeof(TRequest).Name}");
        }

        var handler = _serviceProvider.GetService<IRequestHandler<TRequest, TResult>>() ?? throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");

        return await handler.Handle(request, CancellationToken.None);
    }
}
