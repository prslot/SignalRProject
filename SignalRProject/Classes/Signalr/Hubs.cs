using Microsoft.AspNetCore.SignalR;

namespace SignalRProject.Classes.Signalr
{
    public class Hubs : Hub
    {
        public async Task AddToManagerGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Manager");
        }

        public async Task AddToClientGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Clients");
        }

        public async Task PollHub(string app_identifier)
        {
            await Clients.Groups("Manager").SendAsync("SendPollHub", Context.ConnectionId, app_identifier);
        }

        public async Task RegisterHub(string app_identifier)
        {
            await Clients.Groups("Manager").SendAsync("SendRegisterHub", Context.ConnectionId, app_identifier);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Groups("Manager").SendAsync("DisconnectedHub", Context.ConnectionId);
        }
    }
}
