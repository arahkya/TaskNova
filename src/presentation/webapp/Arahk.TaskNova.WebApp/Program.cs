using Arahk.TaskNova.WebApp.Components;
using Arahk.TaskNova.Lib.Application;
using Arahk.TaskNova.Lib.Infrastructure;
using Arahk.TaskNova.WebApp.ViewModels;
using Arahk.TaskNova.Lib.Domain;
using Arahk.TaskNova.WebApp.Components.Notification;
using Arahk.TaskNova.WebApp.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, TaskNovaAuthenticationStateProvider>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddScoped<IDomainEventHandler<TaskDomainEvent>, CreateTaskViewModel>();
builder.Services.AddSingleton<NotifyHub>();
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

var app = builder.Build();
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapHub<NotifyHub>("/notify");

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Arahk.TaskNova.WebApp.Client._Imports).Assembly);

app.MapGet("/logout", async hld =>
{
    var authStateProvider = hld.RequestServices.GetRequiredService<AuthenticationStateProvider>();
    var authState = await authStateProvider.GetAuthenticationStateAsync();
    var name = authState.User.Identity?.Name;
    
    if (string.IsNullOrEmpty(name)) return;

    await ((TaskNovaAuthenticationStateProvider)authStateProvider).RemoveAuthenticationStateAsync(name);
    
    hld.Response.Redirect("/login");
});

app.Run();


public partial class Program { }