using Microsoft.Extensions.DependencyInjection;

namespace Arahk.TaskNova.Lib.Domain;

public class TaskDomainEvent : DomainEvent<TaskEntity>
{
    public TaskDomainEvent(string message, TaskEntity entity)
    {
        Message = message;
        Entity = entity;
    }
}

public class TaskBoardEntity(IServiceProvider serviceProvider, ITaskRepository taskRepository, IUnitOfWork unitOfWork)
{
    public ITaskRepository TaskRepository { get; } = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    public IUnitOfWork UnitOfWork { get; } = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    public IEnumerable<TaskEntity> Tasks { get; private set; } = [];

    public List<TaskDomainEvent> DomainEvents { get; } = [];

    public async Task<bool> CreateTaskAsync(TaskEntity task)
    {
        var isDuplicate = await IsDuplicateTaskTitleAsync(task);

        if (isDuplicate)
        {
            DomainEvents.Add(new TaskDomainEvent("Task title already exists.", task));

            await serviceProvider.GetRequiredService<DomainEventDispatcher>().DispatchAsync(DomainEvents);

            return false;
        }

        var isCreated = await TaskRepository.CreateTaskAsync(task);

        if (isCreated)
        {
            isCreated = await UnitOfWork.SaveChangesAsync() > 0;

            if (isCreated)
            {
                DomainEvents.Add(new TaskDomainEvent("Task created successfully.", task));
            }
            else
            {
                DomainEvents.Add(new TaskDomainEvent("Failed to save changes after task creation.", task));
            }

            await serviceProvider.GetRequiredService<DomainEventDispatcher>().DispatchAsync(DomainEvents);
        }

        return isCreated;
    }

    private async Task<bool> IsDuplicateTaskTitleAsync(TaskEntity task)
    {
        Tasks = await TaskRepository.GetAllTasksAsync();

        return Tasks.Any(t => t.Title.Equals(task.Title, StringComparison.OrdinalIgnoreCase));
    }
}