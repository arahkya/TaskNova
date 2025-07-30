using System.ComponentModel.DataAnnotations;

namespace Arahk.TaskNova.Lib.Application.Task.CreateTask;

public class CreateTaskRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Range(0, 2)]
    public int Priority { get; set; } = 0; // 0: Low, 1: Medium, 2: High    
}
