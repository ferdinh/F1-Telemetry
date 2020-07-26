using F12020Telemetry.Data;
using System.Collections.Generic;
using System.Linq;

namespace F12020Telemetry.Util.Extensions
{
    public static class LapDataExtensions
    {
        public static IEnumerable<LapData> GetLap(this IReadOnlyCollection<LapData> lapData, int lapNumber)
        {
            return lapData.Where(l => lapNumber.Equals(l.CurrentLapNum) && !l.DriverStatus.Equals(DriverStatus.InGarage));
        }
    }
}