using Arahk.TaskNova.Lib.Application;
using Arahk.TaskNova.Lib.Domain;
using MediatR;

namespace Arahk.TaskNova.WebApp.ViewModels
{
    public class CreateTaskViewModel
    {
        private readonly IMediator Mediator;
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

        public CreateTaskViewModel(IMediator mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Entity.Title = string.Empty;
            Entity.Description = string.Empty;
            Entity.Priority = 1; // Default priority
            Entity.CreatedDate = DateTime.Now;
            Entity.IsCompleted = false;
        }

        public async Task SubmitCreateTask()
        {
            var isSuccess = await Mediator.Send(new CreateTaskRequest
            {
                Title = Title,
                Description = Description
            });

            Console.WriteLine($"Task creation {(isSuccess ? "succeeded" : "failed")}.");
        }
    }
}