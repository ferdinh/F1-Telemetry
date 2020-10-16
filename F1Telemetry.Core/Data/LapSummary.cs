using System;
using System.Collections.Generic;
using System.Linq;

namespace F1Telemetry.Core.Data
{
    /// <summary>
    /// Contains the car and telemetry summary of a lap.
    /// </summary>
    public class LapSummary
    {
        public int LapNumber { get; }
        public float LapTime { get; }
        public SectorTime SectorTime { get; }
        public float BestLapTime { get; }
        public IReadOnlyList<LapData> LapData { get; }
        public IReadOnlyList<CarStatusData> CarStatusData { get; }
        public IReadOnlyList<CarTelemetryData> CarTelemetryData { get; }
        public TyreCompound TyreCompoundUsed { get; } = TyreCompound.Unknown;
        public float ERSEnergyStore { get; }
        public float ERSHarvestedThisLapMGUK { get; }
        public float ERSHarvestedThisLapMGUH { get; }
        public float TotalERSHarvested => ERSHarvestedThisLapMGUH + ERSHarvestedThisLapMGUK;
        public float TotalERSHarvestedPercentage => TotalERSHarvested / CarInfo.F1.MaxDeployableERS;
        public float ERSDeployed { get; set; }
        public float ERSDeployedPercentage => ERSDeployed / CarInfo.F1.MaxDeployableERS;
        public float FuelUsed => CalculateFuelUsage();
        public float TyreWearPercentage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LapSummary"/> class.
        /// </summary>
        /// <param name="latestLapData">The latest lap data given from the game. This is usually the next packet after a new lap.</param>
        /// <param name="lapDatas">The lap datas.</param>
        /// <param name="carStatusDatas">The car status datas.</param>
        /// <param name="carTelemetryDatas">The car telemetry datas.</param>
        public LapSummary(LapData latestLapData, List<LapData> lapDatas, List<CarStatusData> carStatusDatas, List<CarTelemetryData> carTelemetryDatas)
        {
            _ = latestLapData ?? throw new ArgumentNullException(nameof(latestLapData));

            LapData = lapDatas ?? throw new ArgumentNullException(nameof(lapDatas));
            CarStatusData = carStatusDatas ?? throw new ArgumentNullException(nameof(carStatusDatas));
            CarTelemetryData = carTelemetryDatas ?? throw new ArgumentNullException(nameof(carTelemetryDatas));

            LapNumber = latestLapData.CurrentLapNum - 1;
            LapTime = latestLapData.LastLapTime;
            BestLapTime = latestLapData.BestLapTime;

            var lapData = lapDatas.LastOrDefault();

            if (lapData != null)
            {
                var sector1 = lapData.Sector1TimeInMS / 1000.0f;
                var sector2 = lapData.Sector2TimeInMS / 1000.0f;
                var sector3 = LapTime - sector1 - sector2;

                SectorTime = new SectorTime(sector1, sector2, sector3);
            }

            var initialCarStatus = carStatusDatas.FirstOrDefault() ?? new Data.CarStatusData(0, 0);
            var endingCarStatus = carStatusDatas.LastOrDefault();

            if (endingCarStatus != null)
            {
                TyreCompoundUsed = (TyreCompound)endingCarStatus.ActualTyreCompound;
                ERSEnergyStore = endingCarStatus.ErsStoreEnergy;
                ERSDeployed = endingCarStatus.ErsDeployedThisLap;
                ERSHarvestedThisLapMGUK = endingCarStatus.ErsHarvestedThisLapMGUK;
                ERSHarvestedThisLapMGUH = endingCarStatus.ErsHarvestedThisLapMGUH;
                TyreWearPercentage = CalculateTyreWearChange(initialCarStatus, endingCarStatus);
            }
        }

        /// <summary>
        /// Calculates the tyre wear percentage change.
        /// </summary>
        /// <param name="initialCarStatus">The initial car status.</param>
        /// <param name="endingCarStatus">The ending car status.</param>
        /// <returns>Tyre Wear Change; -1 if invalid.</returns>
        private float CalculateTyreWearChange(CarStatusData initialCarStatus, CarStatusData endingCarStatus)
        {
            // Compare the difference of the tyre that has the maximum wear.
            var maxEndTyreValue = endingCarStatus.TyresWear.Max();
            var tyreIndexToCompare = Array.IndexOf(endingCarStatus.TyresWear, maxEndTyreValue);
            var percentageChange = (endingCarStatus.TyresWear.Max() - initialCarStatus.TyresWear[tyreIndexToCompare]) / 100.0f;

            return percentageChange > 0 ? percentageChange : -1.0f;
        }

        private float CalculateFuelUsage()
        {
            var fuelUsed = 0.0f;

            if (CarStatusData.Any())
            {
                var initialFuel = CarStatusData.First().FuelInTank;
                var endFuel = CarStatusData.Last().FuelInTank;

                fuelUsed = Math.Max(0, initialFuel - endFuel);
            }

            return fuelUsed;
        }
    }
}