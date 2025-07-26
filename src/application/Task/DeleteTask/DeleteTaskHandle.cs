using Arahk.TaskNova.Lib.Domain;
using MediatR;

namespace Arahk.TaskNova.Lib.Application.Task.DeleteTask
{
    public class DeleteTaskHandler(IUnitOfWork unitOfWork, ITaskRepository taskRepository) : IRequestHandler<DeleteTaskRequest, bool>
    {
        public async Task<bool> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
        {
            await taskRepository.DeleteTaskAsync(request.TaskId);
            await unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}