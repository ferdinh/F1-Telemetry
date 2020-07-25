using System;
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
        // TODO: Try using Dictionary to hold all of the packets of a particular lap. Use Lap number as the key? It also needs to match the
        // session ID.
        private List<LapData> lapData = new List<LapData>();

        public event EventHandler<NewLapEventArgs> NewLap;

        public event EventHandler LapInterval;

        /// <summary>
        /// Gets or sets the number of laps the driver had done.
        /// </summary>
        /// <value>
        /// The number of laps.
        /// </value>
        public int NumberOfLaps { get; private set; } = 0;

        public int LapIntervalThreshold { get; set; } = 3;

        public IReadOnlyCollection<LapData> LapData
        {
            get { return lapData.AsReadOnly(); }
        }

        private int CurrentLapInterval = 1;

        public IList<CarTelemetryData> CarTelemetryData { get; internal set; } = new List<CarTelemetryData>();

        /// <summary>
        /// Gets the current telemetry.
        /// </summary>
        /// <value>
        /// The current telemetry.
        /// </value>
        public CarTelemetryData CurrentTelemetry => CarTelemetryData.LastOrDefault();

        public void AddLapData(LapData lapData)
        {
            this.lapData.Add(lapData);

            if (NumberOfLaps == 0)
            {
                NumberOfLaps = lapData.CurrentLapNum;
            }
            else if (lapData.CurrentLapNum > NumberOfLaps)
            {
                var lastLapData = LapData.Where(l => (lapData.CurrentLapNum - 1).Equals(l.CurrentLapNum)).ToList().AsReadOnly();
                var lastCarTelemetryData = new List<CarTelemetryData>();

                foreach (var lastLap in lastLapData)
                {
                    lastCarTelemetryData.Add(CarTelemetryData.SingleOrDefault(c => c.SessionTime.Equals(lastLap.SessionTime) && c.SessionUID.Equals(lastLap.SessionUID)));
                }

                var newLapEventArgs = new NewLapEventArgs
                {
                    LastLapNumber = lapData.CurrentLapNum - 1,
                    LastLapTime = lapData.LastLapTime,
                    LastLapData = lastLapData,
                    LastCarTelemetryData = lastCarTelemetryData.AsReadOnly()
                };

                OnNewLap(newLapEventArgs);

                if (CurrentLapInterval == LapIntervalThreshold)
                {
                    OnLapInterval();
                    CurrentLapInterval = 0;
                }

                CurrentLapInterval++;
            }

            NumberOfLaps = lapData.CurrentLapNum;
        }

        protected virtual void OnNewLap(NewLapEventArgs e)
        {
            NewLap?.Invoke(this, e);
        }

        protected virtual void OnLapInterval()
        {
            LapInterval?.Invoke(this, EventArgs.Empty);
        }
    }
}