using Arahk.TaskNova.Lib.Domain;
using Microsoft.EntityFrameworkCore;

namespace Arahk.TaskNova.Lib.Infrastructure;

public class TaskRepository(IUnitOfWork unitOfWork) : ITaskRepository
{
    public async Task<bool> CreateTaskAsync(TaskEntity task)
    {
        var dbContext = (TaskNovaDbContext)unitOfWork;

        await dbContext.Tasks.AddAsync(task);

        return true;
    }

    public Task<bool> DeleteTaskAsync(int taskId)
    {
        var dbContext = (TaskNovaDbContext)unitOfWork;

        var task = dbContext.Tasks.Find(taskId);
        if (task == null)
        {
            return Task.FromResult(false);
        }

        dbContext.Tasks.Remove(task);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
    {
        var dbContext = (TaskNovaDbContext)unitOfWork;

        return Task.FromResult(dbContext.Tasks.AsEnumerable());
    }

    public Task<TaskEntity> GetTaskByIdAsync(int taskId)
    {
        var dbContext = (TaskNovaDbContext)unitOfWork;

        return Task.FromResult(dbContext.Tasks.Find(taskId)!);
    }

    public Task<bool> UpdateTaskAsync(TaskEntity task)
    {
        var dbContext = (TaskNovaDbContext)unitOfWork;

        dbContext.Tasks.Update(task);

        return Task.FromResult(true);
    }
}