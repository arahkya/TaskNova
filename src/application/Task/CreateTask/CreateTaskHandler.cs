using Arahk.TaskNova.Lib.Domain;
using MediatR;

namespace Arahk.TaskNova.Lib.Application.Task.CreateTask;

public class CreateTaskHandler(IUnitOfWork unitOfWork, ITaskRepository taskRepository) : IRequestHandler<CreateTaskRequest, bool>
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

        var isCreateSuccess = await taskRepository.CreateTaskAsync(taskEntity);
        if (isCreateSuccess)
        {
            isCreateSuccess = await unitOfWork.SaveChangesAsync() > 0;
        }

        return isCreateSuccess;
    }
}
