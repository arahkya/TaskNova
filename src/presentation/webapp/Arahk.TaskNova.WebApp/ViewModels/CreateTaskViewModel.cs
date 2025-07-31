using System.ComponentModel;
using Arahk.MyMediatr;
using Arahk.TaskNova.Lib.Application.Task.CreateTask;
using Arahk.TaskNova.Lib.Domain;

namespace Arahk.TaskNova.WebApp.ViewModels
{
    public class CreateTaskViewModel : INotifyPropertyChanged, IDomainEventHandler<TaskDomainEvent>
    {
        private readonly MyMediatr.MyMediatr MyMediatr;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaskEntity Entity { get; set; } = new();

        public ICollection<TaskEntity> TaskEntities { get; set; } = [];

        public string Title
        {
            get => Entity.Title;
            set
            {
                Entity.Title = value ?? string.Empty; // Ensure Title is never null
                OnPropertyChanged(nameof(Title));
            }
        }
        public string Description
        {
            get => Entity.Description;
            set
            {
                Entity.Description = value ?? string.Empty; // Ensure Description is never null
                OnPropertyChanged(nameof(Description));
            }
        }
        public int Priority
        {
            get => Entity.Priority;
            set
            {
                Entity.Priority = value;
                OnPropertyChanged(nameof(Priority));
            }
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

        public async Task HandleAsync(TaskDomainEvent domainEvent)
        {
            await Task.CompletedTask;
        }
    }
}