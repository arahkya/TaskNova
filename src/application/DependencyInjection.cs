using Arahk.MyMediatr;
using Microsoft.Extensions.DependencyInjection;

namespace Arahk.TaskNova.Lib.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMyMediatr(typeof(DependencyInjection).Assembly);

        return services;
    }
}