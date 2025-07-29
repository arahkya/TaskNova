namespace Arahk.TaskNova.Lib.Domain;

public class TaskEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
    public int Priority { get; set; } = 0; // 0: Low, 1: Medium, 2: High
}
