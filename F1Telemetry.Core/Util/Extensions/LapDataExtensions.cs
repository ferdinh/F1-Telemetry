using F1Telemetry.Core.Data;
using System.Collections.Generic;
using System.Linq;

namespace F1Telemetry.Core.Util.Extensions
{
    public static class LapDataExtensions
    {
        public static IEnumerable<LapData> GetLap(this IReadOnlyCollection<LapData> lapData, int lapNumber)
        {
            return lapData.Where(l => lapNumber.Equals(l.CurrentLapNum) && !l.DriverStatus.Equals(DriverStatus.InGarage) && l.PitStatus.Equals(PitStatus.None));
        }
    }
}