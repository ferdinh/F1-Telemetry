using F1Telemetry.Core.Data;
using System.Collections.Generic;
using System.Linq;

namespace F1Telemetry.Core.Util.Extensions
{
    public static class CarTelemetryDataExtensions
    {
        public static IEnumerable<CarTelemetryData> GetForLap(this IList<CarTelemetryData> carTelemetryData, IEnumerable<LapData> lapData)
        {
            var lapCarTelemetryData = new List<CarTelemetryData>();
            var carTelemDataCopy = carTelemetryData.ToArray();

            foreach (var lastLap in lapData)
            {
                lapCarTelemetryData.Add(carTelemDataCopy.SingleOrDefault(c => c.SessionTime.Equals(lastLap.SessionTime) && c.SessionUID.Equals(lastLap.SessionUID)));
            }

            return lapCarTelemetryData;
        }
    }
}