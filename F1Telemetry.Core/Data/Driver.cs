using System;
using System.Collections.Generic;
using System.Linq;

namespace F1Telemetry.Core.Data
{
    /// <summary>
    /// Contains telemetry and driver information.
    /// </summary>
    public class Driver
    {
        // TODO: Try using Dictionary to hold all of the packets of a particular lap. Use Lap number as the key? It also needs to match the
        // session ID.
        private readonly List<LapData> lapData = new List<LapData>();

        public Driver(TelemetryManager manager)
        {
            Manager = manager;
        }

        public event EventHandler<NewLapEventArgs> NewLap;

        public IList<CarTelemetryData> CarTelemetryData { get; internal set; } = new List<CarTelemetryData>();

        public IList<CarStatusData> CarStatusData { get; internal set; } = new List<CarStatusData>();

        /// <summary>
        /// Gets or sets the number of laps the driver had done.
        /// </summary>
        /// <value>
        /// The number of laps.
        /// </value>
        public int CurrentLapNumber { get; private set; } = 0;

        /// <summary>
        /// Gets the current telemetry.
        /// </summary>
        /// <value>
        /// The current telemetry.
        /// </value>
        public CarTelemetryData CurrentTelemetry => CarTelemetryData.LastOrDefault();

        public CarStatusData CurrentCarStatus => CarStatusData.LastOrDefault();

        public LapData CurrentLapData { get; private set; }

        public IReadOnlyCollection<LapData> LapData
        {
            get { return lapData.AsReadOnly(); }
        }

        public TelemetryManager Manager { get; }

        public void AddLapData(LapData lapData)
        {
            CurrentLapData = lapData;
            this.lapData.Add(lapData);

            if (CurrentLapNumber == 0)
            {
                CurrentLapNumber = lapData.CurrentLapNum;
            }
            else if (lapData.CurrentLapNum > CurrentLapNumber)
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


            }

            CurrentLapNumber = lapData.CurrentLapNum;
        }

        public void AddCarStatusData(CarStatusData carStatusData)
        {
            this.CarStatusData.Add(carStatusData);
        }

        protected virtual void OnLapInterval()
        {
            LapInterval?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnNewLap(NewLapEventArgs e)
        {
            NewLap?.Invoke(this, e);
        }
    }

    public class NewLapEventArgs : EventArgs
    {
        public IReadOnlyList<CarTelemetryData> LastCarTelemetryData { get; set; }
        public IReadOnlyList<LapData> LastLapData { get; set; }
        public int LastLapNumber { get; set; }
        public float LastLapTime { get; set; }
    }
}