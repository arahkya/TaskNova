using Arahk.MyMediatr;
using Arahk.TaskNova.Lib.Application.Task.CreateTask;
using Arahk.TaskNova.Lib.Domain;

namespace Arahk.TaskNova.WebApp.ViewModels
{
    public class CreateTaskViewModel : IDomainEventHandler<TaskDomainEvent>
    {
        private readonly MyMediatr.MyMediatr MyMediatr;
        public TaskEntity Entity { get; set; } = new();

        public string Title
        {
            get => Entity.Title;
            set => Entity.Title = value ?? string.Empty; // Ensure Title is never null   
        }
        public string Description
        {
            get => Entity.Description;
            set => Entity.Description = value ?? string.Empty; // Ensure Description is never null
        }
        public int Priority
        {
            get => Entity.Priority;
            set => Entity.Priority = value;
        }
        public DateTime CreateDate => Entity.CreatedDate;
        public bool IsCompleted => Entity.IsCompleted;

        public CreateTaskViewModel(MyMediatr.MyMediatr myMediatr)
        {
            MyMediatr = myMediatr ?? throw new ArgumentNullException(nameof(myMediatr));
            Entity.Title = string.Empty;
            Entity.Description = string.Empty;
            Entity.Priority = 1; // Default priority
            Entity.CreatedDate = DateTime.Now;
            Entity.IsCompleted = false;
        }

        public async Task<Response<bool>> SubmitCreateTask()
        {
            var response = await MyMediatr.ExecuteWithValidateAsync<CreateTaskRequest, bool>(new CreateTaskRequest
            {
                Title = Title,
                Description = Description
            });

            Console.WriteLine($"Task creation {(response.IsSuccess ? "succeeded" : "failed")}.");

            return response;
        }

        public Task HandleAsync(TaskDomainEvent domainEvent)
        {
            return Task.Delay(1); // Placeholder for handling domain events
        }
    }
}