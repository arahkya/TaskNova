namespace Arahk.TaskNova.Lib.Domain;

public interface IDomainEventHandler<TDomainEvent>
{
    Task HandleAsync(TDomainEvent domainEvent);
}

public class DomainEventDispatcher(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task DispatchAsync<TEntity>(IEnumerable<DomainEvent<TEntity>> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handler = _serviceProvider.GetService(typeof(IDomainEventHandler<TaskDomainEvent>)) as IDomainEventHandler<TaskDomainEvent>;

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for domain event type {typeof(TEntity).Name}");
            }

            await handler.HandleAsync(domainEvent as TaskDomainEvent);

            // var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent!.GetType());

            // if (_serviceProvider.GetService(handlerType) is IDomainEventHandler<DomainEvent<TEntity>> handler)
            // {
            //     await handler.HandleAsync(domainEvent);
            // }
        }
    }
}

public abstract class DomainEvent<TEntity>
{
    public string Message { get; init; } = string.Empty;

    public TEntity Entity { get; init; } = default!;
}