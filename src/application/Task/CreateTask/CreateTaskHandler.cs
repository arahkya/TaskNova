using Arahk.TaskNova.Lib.Domain;

namespace Arahk.TaskNova.Lib.Application.Task.CreateTask;

public class CreateTaskHandler(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, ITaskRepository taskRepository)
    : MyMediatr.IRequestHandler<CreateTaskRequest, bool>, IDomainEventHandler<TaskDomainEvent>
{
    public async Task<bool> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var taskEntity = new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            CreatedDate = request.CreatedDate,
            Priority = request.Priority
        };

        var taskBoard = new TaskBoardEntity(serviceProvider, taskRepository, unitOfWork);

        var isCreateSuccess = await taskBoard.CreateTaskAsync(taskEntity);

        return isCreateSuccess;
    }

    public System.Threading.Tasks.Task HandleAsync(TaskDomainEvent domainEvent)
    {
        throw new NotImplementedException();
    }
}
