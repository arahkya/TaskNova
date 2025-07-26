namespace Arahk.TaskNova.Lib.Domain;

public interface ITaskRepository
{
    Task<bool> CreateTaskAsync(TaskEntity task);
    Task<bool> UpdateTaskAsync(TaskEntity task);
    Task<bool> DeleteTaskAsync(Guid taskId);
    Task<TaskEntity> GetTaskByIdAsync(Guid taskId);
    Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
}