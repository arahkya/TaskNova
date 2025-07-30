using System.ComponentModel.DataAnnotations;
using Arahk.MyMediatr;

namespace Arahk.TaskNova.Lib.Application.Task.CreateTask;

public class CreateTaskValidator : IRequestValidator<CreateTaskRequest>
{
    public async Task<bool> ValidateAsync(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        return await System.Threading.Tasks.Task.FromResult(request.Title != "Test");
    }
}