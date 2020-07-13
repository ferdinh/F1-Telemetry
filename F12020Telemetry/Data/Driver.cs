using System.Collections.Generic;
using System.Linq;

namespace F12020Telemetry.Data
{
    /// <summary>
    /// Contains telemetry and driver information.
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// Gets or sets the number of laps the driver had done.
        /// </summary>
        /// <value>
        /// The number of laps.
        /// </value>
        public int NumberOfLaps { get; private set; } = 0;

        public IList<LapData> LapData { get; } = new List<LapData>();
        public IList<CarTelemetryData> CarTelemetryData { get; internal set; } = new List<CarTelemetryData>();

        public CarTelemetryData CurrentTelemetry => CarTelemetryData.LastOrDefault();
    }
}