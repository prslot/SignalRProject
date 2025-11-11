using ClientPlayerNew;
using Microsoft.AspNetCore.SignalR.Client;
using NetCoreAudio;
using NetCoreAudio.Interfaces;
using NetCoreAudio.Players;

Console.WriteLine("Provide a device name.");
var app_identifier = Console.ReadLine();
//Guid uniqueAppCode = Guid.NewGuid();

var SignalRConn = new HubConnectionBuilder()
    .WithUrl("https://localhost:7112/hub")
    .Build();

SignalRConn.On<byte[]>("Notify", async message =>
{
    string FileName = DateTime.Now.ToString("dd_MM_yyyy_HHmmssfff") + "_Audio.mp3";
    File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), FileName), message);

    var player = new AudioPlayer();
    player.PlaybackFinished += OnPlaybackFinished;

    await player.Play(Path.Combine(Directory.GetCurrentDirectory(), FileName));
    Console.WriteLine(player.Playing ? "Playback started of file " + FileName : "Could not start the playback");
});

await SignalRConn.StartAsync();
await SignalRConn.InvokeAsync("AddToClientGroup");
await SignalRConn.InvokeAsync("RegisterHub", app_identifier);

while (true)
{
    await SignalRConn.InvokeAsync("PollHub", app_identifier);
    await Task.Delay(30000);
}

static void OnPlaybackFinished(object sender, EventArgs e)
{
    Console.WriteLine("Playback finished");
}
