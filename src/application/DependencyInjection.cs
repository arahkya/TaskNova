using Arahk.MyMediatr;
using Arahk.TaskNova.Lib.Application.Task.CreateTask;
using Arahk.TaskNova.Lib.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Arahk.TaskNova.Lib.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMyMediatr(typeof(DependencyInjection).Assembly);
        services.AddScoped<DomainEventDispatcher>();
        services.AddScoped<IDomainEventHandler<TaskDomainEvent>, CreateTaskHandler>();

        return services;
    }
}