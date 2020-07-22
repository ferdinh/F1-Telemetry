using System.Collections.Generic;
using System.Linq;

namespace F12020Telemetry.Data
{
    public class NewLapEventArgs : EventArgs
    {
        public int LastLapNumber { get; set; }
        public float LastLapTime { get; set; }
        public IReadOnlyList<LapData> LastLapData { get; set; }
        public IReadOnlyList<CarTelemetryData> LastCarTelemetryData { get; set; }
    }

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

        public IReadOnlyCollection<LapData> LapData
        {
            get { return lapData.AsReadOnly(); }
        }

        public IList<CarTelemetryData> CarTelemetryData { get; internal set; } = new List<CarTelemetryData>();

        /// <summary>
        /// Gets the current telemetry.
        /// </summary>
        /// <value>
        /// The current telemetry.
        /// </value>
        public CarTelemetryData CurrentTelemetry => CarTelemetryData.LastOrDefault();
    }
}