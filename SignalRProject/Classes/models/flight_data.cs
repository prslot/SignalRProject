namespace SignalRProject.Classes.models
{
    public class flight_data
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int DayofMonth { get; set; }
        public int DayOfWeek { get; set; }
        public int DepTime { get; set; }
        public int ArrTime { get; set; }
        public string? UniqueCarrier { get; set; }
        public string? FlightNum { get; set; }
        public int ArrDelay { get; set; } 
        public int DepDelay { get; set; }
        public string? Origin { get; set; }
        public string? Dest { get; set; }
        public int Cancelled { get; set; }
        public int CarrierDelay { get; set; }
        public int WeatherDelay { get; set; }
        public int SecurityDelay { get; set; }
        public int LateAircraftDelay { get; set; }
        public string? GateNumber { get; set; }

        public static flight_data FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');

            flight_data f_data = new flight_data();

            f_data.Year = Convert.ToInt32(values[0]);
            f_data.Month = Convert.ToInt32(values[1]);
            f_data.DayofMonth = Convert.ToInt32(values[2]);
            f_data.DayOfWeek = Convert.ToInt32(values[3]);
            f_data.DepTime = Convert.ToInt32(values[4]);
            f_data.ArrTime = Convert.ToInt32(values[5]);
            f_data.UniqueCarrier = values[6].ToString();
            f_data.FlightNum = values[7].ToString();
            f_data.ArrDelay = Convert.ToInt32(values[8]);
            f_data.DepDelay = Convert.ToInt32(values[9]);
            f_data.Origin = values[10].ToString();
            f_data.Dest = values[11].ToString();
            f_data.Cancelled = Convert.ToInt32(values[12]);
            f_data.CarrierDelay = Convert.ToInt32(values[13]);
            f_data.WeatherDelay = Convert.ToInt32(values[14]);
            f_data.SecurityDelay = Convert.ToInt32(values[15]);
            f_data.LateAircraftDelay = Convert.ToInt32(values[16]);
            f_data.GateNumber = values[17].ToString();

            return f_data;
        }
    }
}
