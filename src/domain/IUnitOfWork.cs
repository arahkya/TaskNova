namespace Arahk.TaskNova.Lib.Domain;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
}
