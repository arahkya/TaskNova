using Arahk.MyMediatr;
using Arahk.TaskNova.Lib.Domain;

namespace Arahk.TaskNova.Lib.Application.Task.GetAllTask;

public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskEntity>>
{
    private readonly ITaskRepository _taskRepository;

    public GetAllTasksHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    }

    public async Task<IEnumerable<TaskEntity>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetAllTasksAsync();
    }
}
