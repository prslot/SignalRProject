using SignalRProject.Classes.Google;
using SignalRProject.Classes.models;

namespace SignalRProject.Classes.Events
{
    public class Arrival
    {

        public static string AnnounceArrival(flight_data data)
        {
            if (data != null)
            {
                string txtAnnoucement = "Vlucht " + data.FlightNum + " met oorsprong " + data.Origin + " is gearriveerd aan de gate";
                string fileName = "flight_" + data.FlightNum + "_arrival_" + data.ArrTime + ".mp3";
                return TextToSpeach.CreаteTextToAudiоFile(txtAnnoucement, fileName);
            }

            return "";

        }

    }
}
