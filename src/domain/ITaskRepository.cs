namespace Arahk.TaskNova.Lib.Domain;

public interface ITaskRepository
{
    Task<bool> CreateTaskAsync(TaskEntity task);
    Task<bool> UpdateTaskAsync(TaskEntity task);
    Task<bool> DeleteTaskAsync(int taskId);
    Task<TaskEntity> GetTaskByIdAsync(int taskId);
    Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
}