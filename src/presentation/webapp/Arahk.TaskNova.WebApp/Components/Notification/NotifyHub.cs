using Microsoft.AspNetCore.SignalR;

namespace Arahk.TaskNova.WebApp.Notification;

public class NotifyHub : Hub
{
    private string _connectionid;

    public async Task SendMessage(string user, string message)
    {
        await Clients.Client(_connectionid).SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        _connectionid = Context.ConnectionId;

        // var userid = Context.User?.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty;
    }
}
