using System.Threading.Tasks;
using Arahk.TaskNova.Lib.Application;
using Arahk.TaskNova.Lib.Infrastructure;
using Arahk.TaskNova.WebApp.Components.Pages;
using Arahk.TaskNova.WebApp.Components.Notification;

namespace Arahk.TaskNova.WebApp.Test;

public class CreateTaskTests : TestContext
{
    [Fact]
    public async Task Test_CreateTask_ValidInput_CreatesTask()
    {
        this.Services.AddSingleton<NotifyHub>();
        this.Services.AddApplicationServices();
        this.Services.AddInfrastructureServices();

        // Arrange
        var createTaskComponent = RenderComponent<CreateTask>();

        // Act
        var pageHeader = createTaskComponent.Find("h1");
        var taskTitleInput = createTaskComponent.Find("input[name='input-title']");
        var taskDescriptionInput = createTaskComponent.Find("input[name='input-detail']");
        var createButton = createTaskComponent.Find("button");

        taskTitleInput.Change("New Task Title");
        taskDescriptionInput.Change("This is a description of the new task.");

        createButton.Click();

        await Task.Delay(100); // Wait for the task to be created and rendered

        // Assert
        pageHeader.MarkupMatches("<h1>Create Task</h1>");

        var tasksList = createTaskComponent.Find(".task-list");
        var tasksListItems = tasksList.QuerySelectorAll("ul li");

        Assert.Single(tasksListItems);
        Assert.Contains("New Task Title", tasksListItems[0].TextContent);
        Assert.Contains("This is a description of the new task.", tasksListItems[0].TextContent);
    }

}