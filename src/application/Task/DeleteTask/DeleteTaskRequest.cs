using MediatR;

namespace Arahk.TaskNova.Lib.Application.Task.DeleteTask
{
    public class DeleteTaskRequest : IRequest<bool>
    {
        public Guid TaskId { get; set; }
    }
}