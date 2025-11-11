using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace SignalRProject.Classes.Signalr
{
    public class Hubs : Hub
    {

        private readonly Regex GateReg = new Regex(@"([a-zA-Z]+)(\d+)");

        public async Task AddToManagerGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Manager");
        }

        public async Task AddToClientGroup(string gate)
        {
            Match result = GateReg.Match(gate);

            string gate_group = result.Groups[1].ToString();
            string gate_number = result.Groups[2].ToString();

            await Groups.AddToGroupAsync(Context.ConnectionId, "Clients_" + gate_group);
        }

        public async Task PollHub(string app_identifier, string gate)
        {
            
            Match result = GateReg.Match(gate);

            string gate_group = result.Groups[1].ToString();
            string gate_number = result.Groups[2].ToString();

            await Clients.Groups("Manager").SendAsync("SendPollHub", Context.ConnectionId, app_identifier, gate_group, gate_number);
        }

        public async Task RegisterHub(string app_identifier, string gate)
        {
            Match result = GateReg.Match(gate);

            string gate_group = result.Groups[1].ToString();
            string gate_number = result.Groups[2].ToString();

            await Clients.Groups("Manager").SendAsync("SendRegisterHub", Context.ConnectionId, app_identifier, gate_group, gate_number);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Groups("Manager").SendAsync("DisconnectedHub", Context.ConnectionId);
        }
    }
}
