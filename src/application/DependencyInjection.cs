using Arahk.TaskNova.Lib.Application.Task.CreateTask;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Arahk.TaskNova.Lib.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            var applicationAssembly = typeof(DependencyInjection).Assembly;

            cfg.RegisterServicesFromAssembly(applicationAssembly);
        });

        services.AddScoped<IRequestHandler<CreateTaskRequest, bool>, CreateTaskHandler>();

        return services;
    }
}