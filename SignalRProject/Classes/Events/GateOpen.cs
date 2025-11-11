using SignalRProject.Classes.Google;
using SignalRProject.Classes.models;

namespace SignalRProject.Classes.Events
{
    public class GateOpen
    {
        public static string AnnounceGateOpen(flight_data data)
        {
            if (data != null)
            {
                string txtAnnoucement = "De gate voor vlucht " + data.FlightNum + " met bestemming " + data.Dest + "is geopend.";
                string fileName = "flight_" + data.FlightNum + "_gate_open_" + data.DepTime + ".mp3";
                return TextToSpeach.CreаteTextToAudiоFile(txtAnnoucement, fileName);
            }

            return "";
        }
    }
}
