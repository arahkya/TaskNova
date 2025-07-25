using MediatR;

namespace Arahk.TaskNova.Lib.Application;

public class CreateTaskRequest : IRequest<bool>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int Priority { get; set; } = 0; // 0: Low, 1: Medium, 2: High    
}
