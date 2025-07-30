using System.ComponentModel.DataAnnotations;
using Arahk.MyMediatr;
using Arahk.TaskNova.Lib.Domain;

namespace Arahk.TaskNova.Lib.Application.Task.CreateTask;

public class CreateTaskValidator(ITaskRepository taskRepository) : IRequestValidator<CreateTaskRequest>
{
    public async Task<bool> ValidateAsync(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var existedTask = await taskRepository.GetAllTasksAsync();
        var isDuplicate = existedTask.Any(p => p.Title == request.Title);

        return await System.Threading.Tasks.Task.FromResult(!isDuplicate);
    }
}