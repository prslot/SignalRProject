
using Microsoft.AspNetCore.SignalR;
using SignalRProject.Classes.Events;
using SignalRProject.Classes.models;
using SignalRProject.Classes.Signalr;
using System.Text.RegularExpressions;

namespace SignalRProject.Classes.Timer
{
    public class FileReader : IHostedService, IDisposable
    {
        private IWebHostEnvironment _webHostEnvironment;
        private int executionCount = 0;
        private readonly ILogger<FileReader> _logger;
        private readonly IHubContext<Hubs> _hubcontext;
        private System.Threading.Timer? _timer = null;

        private readonly Regex GateReg = new Regex(@"([a-zA-Z]+)(\d+)");
        private const int hoursToGate = 1;
        private const int minutesToRepeat = 5;

        public FileReader(ILogger<FileReader> logger, IWebHostEnvironment webHostEnvironment, IHubContext<Hubs> hubContext)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _hubcontext = hubContext;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("FileReader Service running.");

            _timer = new System.Threading.Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(minutesToRepeat));

            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("FileReader Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "input_data/flights.csv");

            List<flight_data> values = File.ReadAllLines(FilePath).Skip(1).Select(v => flight_data.FromCsv(v)).ToList();

            foreach(var datarow in values)
            {
                try
                {
                    string gate_group = "";
                    string gate_number = "";

                    if(!string.IsNullOrWhiteSpace(datarow.GateNumber))
                    {
                        Match result = GateReg.Match(datarow.GateNumber);

                        gate_group = result.Groups[1].ToString();
                        gate_number = result.Groups[2].ToString();
                    }

                    DateTime currDateTime = DateTime.Now;

                    currDateTime = currDateTime.AddSeconds(-currDateTime.Second).AddMilliseconds(-currDateTime.Millisecond).AddMicroseconds(-currDateTime.Microsecond);

                    TimeSpan arrTime = new TimeSpan(datarow.ArrTime / 100, datarow.ArrTime % 100, 0);
                    TimeSpan depTime = new TimeSpan(datarow.DepTime / 100, datarow.DepTime % 100, 0);

                    DateTime arrDateTime = new DateTime(datarow.Year, datarow.Month, datarow.DayofMonth, arrTime.Hours, arrTime.Minutes, arrTime.Seconds);
                    DateTime depDateTime = new DateTime(datarow.Year, datarow.Month, datarow.DayofMonth, depTime.Hours, depTime.Minutes, depTime.Seconds);

                    _logger.LogInformation("check flight: " + datarow.FlightNum + " at arrivaltime: " + arrTime.Hours + ":" + arrTime.Minutes + " and at departure time: " + depTime.Hours + ":" + depTime.Minutes + " at time: " + currDateTime);

                    //datetime must be between Now and Now + minutesToRepeat
                    if (arrDateTime >= currDateTime && arrDateTime < currDateTime.AddMinutes(minutesToRepeat))
                    {
                        string fileLoc = Arrival.AnnounceArrival(datarow);
                        var bytesMP3 = File.ReadAllBytes(fileLoc);
                        _logger.LogInformation("Arrival flight: " + datarow.FlightNum + " at arrivaltime: " + arrTime.Hours + ":" + arrTime.Minutes);
                        _hubcontext.Clients.All.SendAsync("Notify", bytesMP3);
                        continue;
                    }

                    //datetime must be within 1 hour
                    if(depDateTime >= currDateTime.AddHours(hoursToGate) && depDateTime < currDateTime.AddHours(hoursToGate).AddMinutes(minutesToRepeat))
                    {
                        string fileLoc = GateOpen.AnnounceGateOpen(datarow);
                        var bytesMP3 = File.ReadAllBytes(fileLoc);
                        _logger.LogInformation("Gate open flight: " + datarow.FlightNum + " at departure time: " + depTime.Hours + ":" + depTime.Minutes);
                        _hubcontext.Clients.Groups("Clients_" + gate_group).SendAsync("Notify", bytesMP3);
                        continue;
                    }

                    //datetime must be within 30 minutes
                    if (depDateTime >= currDateTime.AddMinutes(30) && depDateTime < currDateTime.AddMinutes(30 + minutesToRepeat))
                    {
                        string fileLoc = FinalCall.AnnounceFinalCall(datarow);
                        var bytesMP3 = File.ReadAllBytes(fileLoc);
                        _logger.LogInformation("Final call flight: " + datarow.FlightNum + " at departure time: " + depTime.Hours + ":" + depTime.Minutes);
                        _hubcontext.Clients.Groups("Clients_" + gate_group).SendAsync("Notify", bytesMP3);
                        continue;
                    }


                }
                catch (Exception)
                {
                    continue;
                }
            }

            _logger.LogInformation("FileReader Service is working. Count: {Count}", count);
        }
    }
}
