using Arahk.TaskNova.Lib.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Arahk.TaskNova.Lib.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<TaskNovaDbContext>(options =>
            options.UseInMemoryDatabase("TaskNovaDb"));

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUnitOfWork, TaskNovaDbContext>();

        return services;
    }
}
