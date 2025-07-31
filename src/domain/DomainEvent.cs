using Microsoft.Extensions.DependencyInjection;

namespace Arahk.TaskNova.Lib.Domain;

public interface IDomainEventHandler<in TDomainEvent>
{
    Task HandleAsync(TDomainEvent domainEvent);
}

public class DomainEventDispatcher(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task DispatchAsync<TEntity>(IEnumerable<DomainEvent<TEntity>> domainEvents)
    {
        var tasks = new List<Task>();

        foreach (var domainEvent in domainEvents)
        {
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<TaskDomainEvent>>().ToList() ?? [];

            handlers.ForEach(handler =>
            {
                if (handler != null && domainEvent is TaskDomainEvent taskDomainEvent)
                {
                    tasks.Add(handler.HandleAsync(taskDomainEvent));
                }
            });
        }

        await Task.WhenAll(tasks);
    }
}

public abstract class DomainEvent<TEntity>
{
    public string Message { get; init; } = string.Empty;

    public TEntity Entity { get; init; } = default!;
}