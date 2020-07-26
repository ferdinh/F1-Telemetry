using F12020Telemetry.Data;
using System.Collections.Generic;
using System.Linq;

namespace F12020Telemetry.Util.Extensions
{
    public static class CarTelemetryDataExtensions
    {
        public static IEnumerable<CarTelemetryData> GetForLap(this IList<CarTelemetryData> carTelemetryData, IEnumerable<LapData> lapData)
        {
            var lapCarTelemetryData = new List<CarTelemetryData>();

            foreach (var lastLap in lapData)
            {
                lapCarTelemetryData.Add(carTelemetryData.SingleOrDefault(c => c.SessionTime.Equals(lastLap.SessionTime) && c.SessionUID.Equals(lastLap.SessionUID)));
            }

            return lapCarTelemetryData;
        }
    }
}