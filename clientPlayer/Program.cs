using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using NetCoreAudio.Interfaces;
using NetCoreAudio.Players;

namespace clientPlayer
{
    public class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Provide a device name.");
            var app_identifier = Console.ReadLine();
            //Guid uniqueAppCode = Guid.NewGuid();

            var SignalRConn = new HubConnectionBuilder()
                .WithUrl("https://localhost:7112/hub")
                .Build();

            SignalRConn.On<byte[]>("Notify", message =>
            {
                File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(),  DateTime.Now.ToString("dd_MM_yyyy_HHmmssfff") + "_Audio.mp3"), message);
            });

            await SignalRConn.StartAsync();
            await SignalRConn.InvokeAsync("AddToClientGroup");
            await SignalRConn.InvokeAsync("RegisterHub", app_identifier);

            while (true)
            {
                await SignalRConn.InvokeAsync("PollHub", app_identifier);
                await Task.Delay(30000);
            }
        }
    }
}
