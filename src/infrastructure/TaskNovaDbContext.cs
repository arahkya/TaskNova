using Arahk.TaskNova.Lib.Domain;
using Microsoft.EntityFrameworkCore;

namespace Arahk.TaskNova.Lib.Infrastructure;

public class TaskNovaDbContext(DbContextOptions<TaskNovaDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<TaskEntity> Tasks { get; set; }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
