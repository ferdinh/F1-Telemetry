using F1Telemetry.Core.Util.Extensions;
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
        private readonly List<LapData> lapData = new List<LapData>();

        public Driver(TelemetryManager manager)
        {
            Manager = manager;
        }

        public event EventHandler<NewLapEventArgs> NewLap;

        public event EventHandler Pitting;

        public IList<CarStatusData> CarStatusData { get; internal set; } = new List<CarStatusData>();
        public IList<CarTelemetryData> CarTelemetryData { get; internal set; } = new List<CarTelemetryData>();
        public CarStatusData CurrentCarStatus => CarStatusData.LastOrDefault();
        public LapData CurrentLapData { get; private set; }

        /// <summary>
        /// Gets or sets the number of laps the driver had done.
        /// </summary>
        /// <value>
        /// The number of laps.
        /// </value>
        public int CurrentLapNumber { get; private set; }

        /// <summary>
        /// Gets the best lap time for the driver.
        /// </summary>
        /// <value>
        /// The best lap time.
        /// </value>
        public float BestLapTime { get; protected set; }

        public DriverStatusInfo CurrentStatus { get; } = new DriverStatusInfo();

        /// <summary>
        /// Gets the current telemetry.
        /// </summary>
        /// <value>
        /// The current telemetry.
        /// </value>
        public CarTelemetryData CurrentTelemetry => CarTelemetryData.LastOrDefault();

        public IReadOnlyCollection<LapData> LapData
        {
            get { return lapData.ToList().AsReadOnly(); }
        }

        public Dictionary<int, LapSummary> LapSummaries { get; } = new Dictionary<int, LapSummary>();
        public TelemetryManager Manager { get; }

        public void AddCarStatusData(CarStatusData carStatusData)
        {
            this.CarStatusData.Add(carStatusData);
        }

        /// <summary>
        /// Adds the valid lap data.
        /// </summary>
        /// <param name="lapData">The lap data.</param>
        public void AddLapData(LapData lapData)
        {
            CurrentLapData = lapData;
            BestLapTime = lapData.BestLapTime;

            var isDriverPitting = lapData.PitStatus == PitStatus.Pitting && (CurrentStatus.PitStatus != PitStatus.Invalid && CurrentStatus.PitStatus != PitStatus.Pitting);

            if (isDriverPitting)
            {
                OnPitting();
            }

            UpdateDriverStatusInfo(lapData);

            if (CurrentLapNumber == 0)
            {
                CurrentLapNumber = lapData.CurrentLapNum;
            }
            else if (lapData.CurrentLapNum > CurrentLapNumber)
            {
                var previousLapNum = lapData.CurrentLapNum - 1;
                var lastLapData = LapData.Where(l => previousLapNum.Equals(l.CurrentLapNum)).ToList();

                var carTelemetryDataCopy = CarTelemetryData.ToList();
                var carStatusDataCopy = CarStatusData.ToList();

                var lastCarTelemetryData = carTelemetryDataCopy.GetForLap(lastLapData).ToList();
                var lastCarStatusData = carStatusDataCopy.GetForLap(lastLapData).ToList();

                var lapSummary = new LapSummary(lapData, lapDatas: lastLapData, carTelemetryDatas: lastCarTelemetryData, carStatusDatas: lastCarStatusData);

                LapSummaries.Add(CurrentLapNumber, lapSummary);

                var newLapEventArgs = new NewLapEventArgs(lapSummary);
                OnNewLap(newLapEventArgs);

                this.lapData.Clear();
                CarTelemetryData.Clear();
                CarStatusData.Clear();
            }

            if (IsLapDataValid(lapData))
            {
                this.lapData.Add(lapData);
            }

            CurrentLapNumber = lapData.CurrentLapNum;
        }

        /// <summary>
        /// Removes the lap data buffer of the specified lap number.
        /// </summary>
        /// <param name="lapNumber">The lap number.</param>
        public void RemoveOngoingLapData(int lapNumber)
        {
            var lapDataToRemove = lapData.GetLap(lapNumber).ToList();
            var carDataToRemove = CarTelemetryData.GetForLap(lapDataToRemove);

            foreach (var lap in lapDataToRemove)
            {
                lapData.Remove(lap);
            }

            foreach (var item in carDataToRemove)
            {
                CarTelemetryData.Remove(item);
            }
        }

        protected virtual void OnNewLap(NewLapEventArgs e)
        {
            NewLap?.Invoke(this, e);
        }

        protected virtual void OnPitting()
        {
            Pitting?.Invoke(this, EventArgs.Empty);
        }

        private bool IsLapDataValid(LapData lapData)
        {
            return lapData.PitStatus == PitStatus.None && (lapData.DriverStatus.Equals(DriverStatus.FlyingLap) || lapData.DriverStatus.Equals(DriverStatus.OnTrack));
        }

        private void UpdateDriverStatusInfo(LapData lapData)
        {
            CurrentStatus.PitStatus = lapData.PitStatus;
            CurrentStatus.Status = lapData.DriverStatus;
        }
    }

    public class NewLapEventArgs : EventArgs
    {
        public NewLapEventArgs(LapSummary lapSummary)
        {
            LapSummary = lapSummary;
        }

        public LapSummary LapSummary { get; }
    }
}