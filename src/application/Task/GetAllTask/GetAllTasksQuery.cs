using Arahk.TaskNova.Lib.Domain;
using MediatR;

namespace Arahk.TaskNova.Lib.Application.Task.GetAllTask;

public class GetAllTasksQuery : IRequest<IEnumerable<TaskEntity>>
{
}
