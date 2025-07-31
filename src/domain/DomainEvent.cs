using Microsoft.Extensions.DependencyInjection;

namespace Arahk.TaskNova.Lib.Domain;

public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    Task HandleAsync(TDomainEvent domainEvent);
}

public class DomainEventDispatcher(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task DispatchAsync<TDomainEvent>(IEnumerable<TDomainEvent> domainEvents) where TDomainEvent : IDomainEvent
    {
        var tasks = new List<Task>();

        foreach (var domainEvent in domainEvents)
        {
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>().ToList();

            handlers.ForEach(handler =>
            {
                if (domainEvent is IDomainEvent dm)
                {
                    tasks.Add(handler.HandleAsync((TDomainEvent)dm));
                }
            });
        }

        await Task.WhenAll(tasks);
    }
}

public interface IDomainEvent
{
    string Message { get; }
}

public abstract class DomainEvent<TEntity> : IDomainEvent
{
    public string Message { get; protected init; } = string.Empty;

    public TEntity Entity { get; init; } = default!;
}