using SignalRProject.Classes.Google;
using SignalRProject.Classes.models;

namespace SignalRProject.Classes.Events
{
    public class FinalCall
    {
        public static string AnnounceFinalCall(flight_data data)
        {
            if (data != null)
            {
                string txtAnnoucement = "Laatste omroep voor vlucht " + data.FlightNum + " met bestemming " + data.Dest + ".";
                string fileName = "flight_" + data.FlightNum + "_final_call_" + data.DepTime + ".mp3";
                return TextToSpeach.CreаteTextToAudiоFile(txtAnnoucement, fileName);
            }

            return "";
        }
    }
}
