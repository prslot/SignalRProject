using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clientPlayer
{
    internal class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Provide a device name.");
            var app_identifier = Console.ReadLine();
            //Guid uniqueAppCode = Guid.NewGuid();

            var SignalRConn = new HubConnectionBuilder()
                .WithUrl("https://localhost:7112/hub")
                .Build();

            await SignalRConn.StartAsync();
            await SignalRConn.InvokeAsync("RegisterHub", app_identifier);

            while (true)
            {
                await SignalRConn.InvokeAsync("PollHub", app_identifier);
                await Task.Delay(30000);
            }
        }
    }
}
