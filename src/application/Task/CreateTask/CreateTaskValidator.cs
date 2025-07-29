using Arahk.MyMediatr;

namespace Arahk.TaskNova.Lib.Application.Task.CreateTask;

public class CreateTaskValidator : IRequestValidator<CreateTaskRequest>
{
    public async Task<bool> ValidateAsync(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        // Example validation logic
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return false; // Title is required
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return false; // Description is required
        }

        // Add more validation rules as needed
        await System.Threading.Tasks.Task.CompletedTask; // Simulate async operation

        return true; // Validation passed
    }
}