using Arahk.TaskNova.Lib.Domain;
using MediatR;

namespace Arahk.TaskNova.Lib.Application;

public class GetAllTasksQuery : IRequest<IEnumerable<TaskEntity>>
{
}
