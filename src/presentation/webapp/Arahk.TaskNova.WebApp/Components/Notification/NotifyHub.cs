using Arahk.TaskNova.WebApp.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Arahk.TaskNova.WebApp.Components.Notification;

public class NotifyHub(AuthenticationStateProvider authenticationStateProvider) : Hub
{
    private readonly TaskNovaAuthenticationStateProvider _authenticationStateProvider = (TaskNovaAuthenticationStateProvider)authenticationStateProvider;
    
    public async Task SendMessage(string user, string message)
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var connectionId = authState.User.Claims.FirstOrDefault(c => c.Type == "connectionId")?.Value;
        
        await Clients.Client(connectionId!).SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
            
        await _authenticationStateProvider.UpdateConnectionIdAsync("", Context.ConnectionId);
        
        System.Diagnostics.Debug.WriteLine($"Connected: {Context.ConnectionId}");
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        
        System.Diagnostics.Debug.WriteLine($"Disconnected: {Context.ConnectionId}, Exception: {exception?.Message}");
    }
}
